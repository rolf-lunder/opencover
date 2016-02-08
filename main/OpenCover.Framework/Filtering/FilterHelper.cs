﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCover.Framework.Filtering
{
    internal static class FilterHelper
    {
        internal static string WrapWithAnchors(this string data)
        {
            return String.Format("^({0})$", data);
        }

        internal static string ValidateAndEscape(this string match, string notAllowed = @"\[]")
        {
            if (match.IndexOfAny(notAllowed.ToCharArray()) >= 0) 
                throw new InvalidOperationException(String.Format("The string is invalid for an filter name {0}", match));
            return match.Replace(@"\", @"\\").Replace(@".", @"\.").Replace(@"*", @".*");
        }

        internal static IList<AssemblyAndClassFilter> GetMatchingFiltersForAssemblyName(this IEnumerable<AssemblyAndClassFilter> filters, string assemblyName)
        {
            var matchingFilters = filters
                .Where(filter => filter.IsMatchingAssemblyName(assemblyName)).ToList();
            return matchingFilters;
        }

        internal static IList<AssemblyAndClassFilter> GetMatchingFiltersForProcessName(this IEnumerable<AssemblyAndClassFilter> filters, string processName)
        {
            var matchingFilters = filters
                .Where(filter => filter.IsMatchingProcessName(processName)).ToList();
            return matchingFilters;
        }

        internal static void AddRange<T> (this ICollection<T> collection, IEnumerable<T> range) {
            if (collection != null && range != null) {
                foreach (var item in range)
                {
                    collection.Add(item);
                }
            }
        }

        internal static void AddFilters(this ICollection<RegexFilter> target, IEnumerable<string> filters, bool isRegexFilter)
        {
            if (filters == null)
                return;

            foreach (var filter in filters.Where(x => x != null))
            {
                RegexFilter regexFilter;
                if (isRegexFilter)
                {
                    regexFilter = new RegexFilter(filter, false);
                }
                else
                {
                    regexFilter = new RegexFilter(filter.ValidateAndEscape(@"[]"));
                }

                target.Add(regexFilter);
            }
        }
    }
}
