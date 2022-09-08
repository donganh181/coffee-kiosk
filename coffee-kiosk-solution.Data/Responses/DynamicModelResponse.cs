using System;
using System.Collections.Generic;
using System.Text;

namespace coffee_kiosk_solution.Data.Responses
{
    [Serializable]
    public class DynamicModelResponse<T>
    {
        public PagingMetaData Metadata { get; set; }
        public List<T> Data { get; set; }
    }
    [Serializable]
    public class PagingMetaData
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
    }
}
