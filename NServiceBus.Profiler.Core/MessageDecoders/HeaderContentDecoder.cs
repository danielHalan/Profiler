﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NServiceBus.Profiler.Common.Models;
using Newtonsoft.Json;

namespace NServiceBus.Profiler.Core.MessageDecoders
{
    public class HeaderContentDecoder : IContentDecoder<IList<HeaderInfo>>
    {
        private readonly IContentDecoder<string> _stringDecoder;

        public HeaderContentDecoder(IContentDecoder<string> stringDecoder)
        {
            _stringDecoder = stringDecoder;
        }

        public DecoderResult<IList<HeaderInfo>> Decode(byte[] headers)
        {
            if (headers != null && headers.Length != 0)
            {
                var headerAsString = _stringDecoder.Decode(headers);
                if (headerAsString.IsParsed)
                {
                    var headerAsJson = TryParseJson(headerAsString.Value);
                    if (headerAsJson.IsParsed)
                    {
                        return headerAsJson;
                    }

                    var headerAsXml = TryParseXml(headerAsString.Value);
                    if (headerAsXml.IsParsed)
                    {
                        return headerAsXml;
                    }
                }
            }

            return new DecoderResult<IList<HeaderInfo>>();
        }

        private DecoderResult<IList<HeaderInfo>> TryParseJson(string value)
        {
            try
            {
                var json = JsonConvert.DeserializeObject<IList<HeaderInfo>>(value);
                return new DecoderResult<IList<HeaderInfo>>(json, json != null);
            }
            catch
            {
                return new DecoderResult<IList<HeaderInfo>>();
            }
        }

        private DecoderResult<IList<HeaderInfo>> TryParseXml(string value)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(HeaderInfo[]));
                var deserialized = (HeaderInfo[])serializer.Deserialize(new StringReader(value));
                return new DecoderResult<IList<HeaderInfo>>(deserialized);
            }
            catch
            {
                return new DecoderResult<IList<HeaderInfo>>();
            }
        }

        DecoderResult IContentDecoder.Decode(byte[] content)
        {
            return Decode(content);
        }
    }
}