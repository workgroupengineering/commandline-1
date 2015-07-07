﻿// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See doc/License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine
{
    /// <summary>
    /// Provides convenience extension methods for <see cref="CommandLine.ParserResult{T}"/>.
    /// </summary>
    public static class ParserResultExtensions
    {
        /// <summary>
        /// Executes <see cref="Action{T}"/> if <see cref="CommandLine.ParserResult{T}"/> contains
        /// parsed values.
        /// </summary>
        /// <typeparam name="T">Type of the target instance built with parsed value.</typeparam>
        /// <param name="result">An <see cref="CommandLine.ParserResult{T}"/> instance.</param>
        /// <param name="action">The <see cref="Action{T}"/> to execute.</param>
        /// <returns>The same <see cref="result"/> instance.</returns>
        public static ParserResult<T> WithParsed<T>(this ParserResult<T> result, Action<T> action)
        {
            var parsed = result as Parsed<T>;
            if (parsed != null)
            {
                action(parsed.Value);
            }
            return result;
        }

        /// <summary>
        /// Executes <see cref="Action{T}"/> if parsed values are of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the target instance built with parsed value.</typeparam>
        /// <param name="result">An verb result instance.</param>
        /// <param name="action">The <see cref="Action{T}"/> to execute.</param>
        /// <returns>The same <see cref="result"/> instance.</returns>
        public static ParserResult<object> WithParsed<T>(this ParserResult<object> result, Action<T> action)
        {
            var parsed = result as Parsed<object>;
            if (parsed != null)
            {
                if (result.Value.GetType() == typeof(T))
                {
                    action((T)parsed.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// Executes <see cref="Action{IEnumerable{Error}}"/> if <see cref="CommandLine.ParserResult{T}"/> lacks
        /// parsed values and contains errors.
        /// </summary>
        /// <typeparam name="T">Type of the target instance built with parsed value.</typeparam>
        /// <param name="result">An <see cref="CommandLine.ParserResult{T}"/> instance.</param>
        /// <param name="action">The <see cref="Action{IEnumerable{Error}}"/> to execute.</param>
        /// <returns>The same <see cref="result"/> instance.</returns>
        public static ParserResult<T> WithNotParsed<T>(this ParserResult<T> result, Action<IEnumerable<Error>> action)
        {
            var notParsed = result as NotParsed<T>;
            if (notParsed != null)
            {
                action(notParsed.Errors);
            }
            return result;
        }

        /// <summary>
        /// Provides a way to transform result data into another value.
        /// </summary>
        /// <typeparam name="TSource">Type of the target instance built with parsed value.</typeparam>
        /// <typeparam name="TResult">The type of the new value.</typeparam>
        /// <param name="result">An <see cref="CommandLine.ParserResult{T}"/> instance.</param>
        /// <param name="parsedFunc">Lambda executed on successful parsing.</param>
        /// <param name="notParsedFunc">Lambda executed on failed parsing.</param>
        /// <returns>The new value.</returns>
        public static TResult Return<TSource, TResult>(this ParserResult<TSource> result,
            Func<TSource, TResult> parsedFunc,
            Func<IEnumerable<Error>, TResult> notParsedFunc)
        {
            var parsed = result as Parsed<TSource>;
            if (parsed != null)
            {
                return parsedFunc(parsed.Value);
            }
            return notParsedFunc(((NotParsed<TSource>)result).Errors);
        }

        /// <summary>
        /// Provides a way to transform result data into another value.
        /// </summary>
        /// <typeparam name="T1">First verb type.</typeparam>
        /// <typeparam name="T2">Second verb type.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result">The result in verb scenario.</param>
        /// <param name="parsedFunc1">Lambda executed on successful parsing of <see cref="T1"/>.</param>
        /// <param name="parsedFunc2">Lambda executed on successful parsing of <see cref="T2"/>.</param>
        /// <param name="notParsedFunc">Lambda executed on failed parsing.</param>
        /// <returns>The new value.</returns>
        public static TResult Return<T1, T2, TResult>(this ParserResult<object> result,
            Func<T1, TResult> parsedFunc1,
            Func<T2, TResult> parsedFunc2,
            Func<IEnumerable<Error>, TResult> notParsedFunc)
        {
            var parsed = result as Parsed<object>;
            if (parsed != null)
            {
                if (parsed.Value.GetType() == typeof(T1))
                {
                    return parsedFunc1((T1)parsed.Value);
                }
                if (parsed.Value.GetType() == typeof(T2))
                {
                    return parsedFunc2((T2)parsed.Value);
                }
                throw new InvalidOperationException();
            }
            return notParsedFunc(((NotParsed<object>)result).Errors);
        }

        /// <summary>
        /// Provides a way to transform result data into another value.
        /// </summary>
        /// <typeparam name="T1">First verb type.</typeparam>
        /// <typeparam name="T2">Second verb type.</typeparam>
        /// <typeparam name="T3">Third verb type.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result">The result in verb scenario.</param>
        /// <param name="parsedFunc1">Lambda executed on successful parsing of <see cref="T1"/>.</param>
        /// <param name="parsedFunc2">Lambda executed on successful parsing of <see cref="T2"/>.</param>
        /// <param name="parsedFunc3">Lambda executed on successful parsing of <see cref="T2"/>.</param>
        /// <param name="notParsedFunc">Lambda executed on failed parsing.</param>
        /// <returns>The new value.</returns>
        public static TResult Return<T1, T2, T3, TResult>(this ParserResult<object> result,
            Func<T1, TResult> parsedFunc1,
            Func<T2, TResult> parsedFunc2,
            Func<T3, TResult> parsedFunc3,
            Func<IEnumerable<Error>, TResult> notParsedFunc)
        {
            var parsed = result as Parsed<object>;
            if (parsed != null)
            {
                if (parsed.Value.GetType() == typeof(T1))
                {
                    return parsedFunc1((T1)parsed.Value);
                }
                if (parsed.Value.GetType() == typeof(T2))
                {
                    return parsedFunc2((T2)parsed.Value);
                }
                if (parsed.Value.GetType() == typeof(T3))
                {
                    return parsedFunc3((T3)parsed.Value);
                }
                throw new InvalidOperationException();
            }
            return notParsedFunc(((NotParsed<object>)result).Errors);
        }
    }
}