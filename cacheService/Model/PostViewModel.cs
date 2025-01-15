﻿namespace cacheService.Model
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public int Price { get; set; }
    }
}
