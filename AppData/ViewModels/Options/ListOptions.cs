using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.Options
{
    public class ListOptions
    {
        /// <summary>
        /// chỉ số trang bắt đầu từ 1
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// số bản ghi trên mỗi trang
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// tổng số bản ghi
        /// </summary>
        public int AllRecordCount { get; set; }

        /// <summary>
        /// đếm số trang
        /// </summary>
        public int PageCount { get { return AllRecordCount / PageSize + (AllRecordCount % PageSize > 0 ? 1 : 0); } }

        /// <summary>
        /// số bản ghi bỏ qua khi phân trang
        /// </summary>
        public int SkipCalc { get { return (PageIndex - 1) * PageSize; } }

        /// <summary>
        /// sắp xếp tăng dần theo trường
        /// </summary>
        public string AscByProperty { get; set; }

        /// <summary>
        /// sắp xếp giảm dần theo trường
        /// </summary>
        public string DescByProperty { get; set; }

        /// <summary>
        /// gen ra các nút phân trang, ... là -1
        /// </summary>
        public List<int> PaginationIndexes
        {
            get
            {
                List<int> list = new List<int>();
                if (AllRecordCount <= 0) return list;
                if (PageSize <= 0) return list;
                int _pageCount = PageCount;
                // gen ra: 1,...,[page - 2], [page - 1], [page], [page + 1], [page + 2]...,pageCount
                int _addPage = PageIndex - 2;
                if (_addPage >= 1 && _addPage <= _pageCount) list.Add(_addPage);
                _addPage = PageIndex - 1;
                if (_addPage >= 1 && _addPage <= _pageCount) list.Add(_addPage);
                _addPage = PageIndex;
                if (_addPage >= 1 && _addPage <= _pageCount) list.Add(_addPage);
                _addPage = PageIndex + 1;
                if (_addPage >= 1 && _addPage <= _pageCount) list.Add(_addPage);
                _addPage = PageIndex + 2;
                if (_addPage >= 1 && _addPage <= _pageCount) list.Add(_addPage);

                if (list.Count > 0)
                {
                    if (list[0] > 1)
                    {
                        if (list[0] > 2)
                        {
                            list.Insert(0, -1); // separator: ...
                        }
                        list.Insert(0, 1);
                    }
                }
                if (list[list.Count - 1] < _pageCount)
                {
                    if (list[list.Count - 1] < _pageCount - 1)
                    {
                        list.Insert(list.Count, -1); // separator: ...
                    }
                    list.Insert(list.Count, _pageCount);
                }

                return list;
            }
        }
    }
}
