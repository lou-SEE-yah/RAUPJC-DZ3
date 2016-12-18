using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Todo
{
    public class TodoSqlRepository : ITodoRepository
    {
        // to do poruke koje salju iznimke
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public void Add(TodoItem todoItem)
        {
            var exists = _context.TodoItem.Find(todoItem.Id);
            if (exists != null)
            {
                throw new DuplicateTodoItemException("Duplicate id: {" + todoItem.Id + "}");
            }
            _context.TodoItem.Add(todoItem);
            _context.SaveChanges();
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem item = _context.TodoItem.Where(s => s.Id.Equals(todoId)).FirstOrDefault();
            if (item != null && item.UserId != userId)
            {
                throw new TodoAccessDeniedException(userId, todoId);
            }
            return item;
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            List<TodoItem> items =
                _context.TodoItem.Where(s => s.UserId.Equals(userId) && s.IsCompleted == false).OrderByDescending(s => s.DateCreated).ToList();
            return items;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            List<TodoItem> items =
                _context.TodoItem.Where(s => s.UserId.Equals(userId)).OrderByDescending(s => s.DateCreated).ToList();
            return items;
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            List<TodoItem> items =
                _context.TodoItem.Where(s => s.UserId.Equals(userId) && s.IsCompleted == true).OrderByDescending(s => s.DateCreated).ToList();
            return items;
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            List<TodoItem> items = _context.TodoItem.Where(s => s.UserId.Equals(userId) && filterFunction(s)).ToList();
            return items;
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem item = _context.TodoItem.Find(todoId);
            if (item != null)
            {
                if (item.UserId.Equals(userId))
                {
                    item.MarkAsCompleted();
                    _context.SaveChanges();
                    return true;
                }
                throw new TodoAccessDeniedException("Access denied.");
            }
            return false;
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem item = _context.TodoItem.Find(todoId);
            if (item != null)
            {
                if (item.UserId.Equals(userId))
                {
                    _context.TodoItem.Remove(item);
                    //_context.SaveChanges();
                    return true;
                }
                throw new TodoAccessDeniedException("Access denied.");
            }
            return false;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            TodoItem item = _context.TodoItem.Find(todoItem.Id);
            if (item == null)
            {
                Add(todoItem);
                _context.SaveChanges();
                return;
            }
            if (item.UserId.Equals(userId))
            {
                Remove(item.Id, item.UserId);
                Add(todoItem);
                _context.SaveChanges();
                return;
            }
            throw new TodoAccessDeniedException("Access denied.");
        }
    }
}
