using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class JournalRepository : DatabaseConnector, IRepository<Journal>
    {
        public JournalRepository(string connectionString) : base(connectionString) { }
    }
}