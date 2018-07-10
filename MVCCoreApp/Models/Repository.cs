using System.Linq;

namespace MVCCoreApp.Models
{
    public class Repository
    {
        private ApplicationDbContext context;

        // injecting the context dependency to the repository constructor
        public Repository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        // mapping the context queryables 
        public IQueryable<Thing> Things => context.Things;


        // save functionality for things (add or edit)
        public void SaveThing(Thing thing)
        {
            if (thing.Id == 0)
            {
                context.Things.Add(thing);
            }
            else
            {
                Thing dbEntry = context.Things.FirstOrDefault(t => t.Id == thing.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = thing.Name;
                }
            }
            context.SaveChanges();
        }

        // delete things
        public Thing DeleteThing(int thingId)
        {
            Thing dbEntry = context.Things.FirstOrDefault(t => t.Id == thingId);
            if (dbEntry != null)
            {
                context.Things.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

    }
}
