using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using OpenTracing.Propagation;


namespace LibWebAPI.Tracing
{
    internal class RequestHeadersExtractAdapter : ITextMap
    {
        private readonly IHeaderDictionary _headers;

        public RequestHeadersExtractAdapter(IHeaderDictionary headers)
        {
            _headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }

        public void Set(string key, string value)
        {
            throw new NotSupportedException("This class should only be used with ITracer.Extract");
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (KeyValuePair<string, StringValues> kvp in _headers)
            {
                yield return new KeyValuePair<string, string>(kvp.Key, kvp.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
