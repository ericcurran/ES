using System;

namespace BusinessLogic
{
    internal class RecordFileName
    {
       
        public RecordFileName(string recordFileName)
        {
            ParseFileNameData(recordFileName?.Split('.')?[0]);
        }

        public string ClaimNumber { get; private set; }
        public string BundleNumber { get; private set; }
        public int PageNumber { get; private set; }
        public int OrderNumber { get; private set; }

        private void ParseFileNameData(string fileName)
        {
            var data = fileName?.Split('-');
            if (data == null) return;
            if (data.Length > 0)
            {
                ClaimNumber = data[0];
            }
            if (data.Length > 1)
            {
                (string bundleNum, int pageNum) = GetBundleAndPageNums(data[1]);
                BundleNumber = bundleNum;
                PageNumber = pageNum;
            }
            if (data.Length > 2)
            {
                int.TryParse(data[2], out int orderNum);
                OrderNumber = orderNum;
            }
        }

        private (string bundleNum, int pageNum) GetBundleAndPageNums(string value)
        {
            if (string.IsNullOrEmpty(value)) return (default(string), default(int));
            var data = value.Split('_');
            if (data.Length != 2) return (default(string), default(int));
            int.TryParse(data[1], out int pageNum);
            return (data[0], pageNum);
        }
    }
}