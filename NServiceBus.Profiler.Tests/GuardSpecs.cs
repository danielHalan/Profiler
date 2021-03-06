﻿using System;
using Machine.Specifications;
using NServiceBus.Profiler.Common;

namespace NServiceBus.Profiler.Tests.Utilities
{
    [Subject("guard")]
    public abstract class with_a_guard_check
    {
        protected static GuardTest GuardTest;
        protected static Exception Error;

        Establish context = () => { GuardTest = new GuardTest(); };
    }

    [Subject("guard")]
    public class when_checking_in_range_values : with_a_guard_check
    {
        It should_not_throw_when_values_are_inside_the_range = () => GuardTest.AddRangeExclusive(15);
        It should_not_throw_when_values_equal_min_expected_values_inclusive = () => GuardTest.AddRangeInclusive(10);
        It should_not_throw_when_values_equal_max_expected_values_inclusive = () => GuardTest.AddRangeInclusive(20);
        It should_not_throw_when_values_equal_min_expected_values_exclusive = () => GuardTest.AddRangeExclusive(11);
        It should_not_throw_when_values_equal_max_expected_values_exclusive = () => GuardTest.AddRangeExclusive(19);
    }

    [Subject("guard")]
    public class when_checking_out_of_range_values : with_a_guard_check
    {
        It throws_when_inclusive_range_check_not_valid = () => Error = Catch.Exception(() => GuardTest.AddRangeExclusive(0));
        It throws_when_exclusive_range_check_not_valid = () => Error = Catch.Exception(() => GuardTest.AddRangeExclusive(20));
        It throws_when_value_equal_min_expected_values_exclusive = () => Error = Catch.Exception(() => GuardTest.AddRangeExclusive(10));
        It throws_when_value_equal_max_expected_values_exclusive = () => Error = Catch.Exception(() => GuardTest.AddRangeExclusive(20));
        It throws_when_value_equal_max_expected_values_inclusive = () => Error = Catch.Exception(() => GuardTest.AddRangeInclusive(21));
        It throws_when_value_equal_min_expected_values_inclusive = () => Error = Catch.Exception(() => GuardTest.AddRangeInclusive(9));
    }

    [Subject("guard")]
    public class when_null_not_passed_in : with_a_guard_check
    {
        Because of = () => Error = Catch.Exception(() => GuardTest.Run("not null string"));

        It should_not_throw_exceptions = () => Error.ShouldBeNull();
    }

    [Subject("guard")]
    public class when_null_passed_in : with_a_guard_check
    {
        Because of = () => Error = Catch.Exception(() => GuardTest.Run(null));

        It throws_when_variable_is_null = () => Error.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject("guard")]
    public class when_empty_string_passed_in : with_a_guard_check
    {
        Because of = () => Error = Catch.Exception(() => GuardTest.Run(string.Empty));

        It throws_when_variable_is_empty_string = () => Error.ShouldBeOfType<ArgumentException>();
    }

    [Subject("guard")]
    public class when_checking_wrong_true_conditions: with_a_guard_check
    {
        Because of = () => Error = Catch.Exception(() => GuardTest.IsTrue(false));

        It throws_invalid_operations_exception = () => Error.ShouldBeOfType<InvalidOperationException>();
    }

    [Subject("guard")]
    public class when_checking_right_true_conditions : with_a_guard_check
    {
        Because of = () => Error = Catch.Exception(() => GuardTest.IsTrue(true));

        It should_not_throw_any_exceptions = () => Error.ShouldBeNull();
    }

    [Subject("guard")]
    public class when_checking_wrong_false_conditions : with_a_guard_check
    {
        Because of = () => Error = Catch.Exception(() => GuardTest.IsFalse(true));

        It throws_invalid_operations_exception = () => Error.ShouldBeOfType<InvalidOperationException>();
    }

    public class when_checking_right_false_conditions : with_a_guard_check
    {
        Because of = () => Error = Catch.Exception(() => GuardTest.IsFalse(false));

        It should_not_throw_any_exceptions = () => Error.ShouldBeNull();
    }

    public class GuardTest
    {
        public void Run(string testName)
        {
            Guard.NotNull(() => testName, testName);
            Guard.NotNullOrEmpty(() => testName, testName);
        }

        public void AddRangeExclusive(int x)
        {
            Guard.NotOutOfRangeExclusive(() => x, x, 10, 20);
        }

        public void AddRangeInclusive(int x)
        {
            Guard.NotOutOfRangeInclusive(() => x, x, 10, 20);
        }

        public void IsTrue(bool value)
        {
            Guard.True(value);
        }

        public void IsFalse(bool value)
        {
            Guard.False(value);
        }
    }

}