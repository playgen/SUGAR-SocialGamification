using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayGen.SGA.ClientAPI.Extensions
{
    public static class UriBuilderExtensions
    {
        /// <summary>
        /// Builds a Query string to pass to the API from an objects of objects. 
        /// </summary>
        /// <param name="uri">Part of t</param>
        /// <param name="objects">Array of objects to be serialised</param>
        /// <param name="formatString">This expects a composite format string in the form of param={0}</param>
        /// <returns></returns>
        public static UriBuilder AppendQueryParameters<T>(this UriBuilder uri, T[] objects, string formatString)
        {

            foreach (var obj in objects)
            {
                var queryToAppend = string.Format(formatString, obj);
                if (uri.Query != null && uri.Query.Length > 1)
                    uri.Query = uri.Query.Substring(1) + "&" + queryToAppend;
                else
                    uri.Query = queryToAppend;
            }
            return uri;
        }
    }
}
