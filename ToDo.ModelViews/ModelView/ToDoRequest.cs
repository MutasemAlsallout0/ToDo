
namespace ToDo.ModelViews.ModelView
{
    public class ToDoRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ImageString { get; set; }
        public string Content { get; set; }
        public int IsSigned { get; set; }
        public bool IsRead { get; set; }

    }
}
