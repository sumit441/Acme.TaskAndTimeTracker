using System.Collections.Generic;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }

        public PagedResultDto()
        {
        }

        public PagedResultDto(int totalCount, List<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
