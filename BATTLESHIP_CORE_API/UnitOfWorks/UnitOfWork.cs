using BATTLESHIP_CORE_API.Repositories;
using BATTLESHIP_CORE_EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTrans();
        void Commit();
        void RollBack();
        int SaveChanges();

        IGameRepository Games { get; }
        IShipRepository Ships { get; }
        IActionShipInstallRepository ActionShipInstalls { get; }
        IActionMoveRepository ActionMoves { get; }

    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _db;

        public UnitOfWork(DBContext db)
        {
            _db = db;
            Games = new GameRepository(db);
            Ships = new ShipRepository(db);
            ActionShipInstalls = new ActionShipInstallRepository(db);
            ActionMoves = new ActionMoveRepository(db);
        }

        public IGameRepository Games { get; private set; }
        public IShipRepository Ships { get; private set; }
        public IActionShipInstallRepository ActionShipInstalls { get; private set; }
        public IActionMoveRepository ActionMoves { get; private set; }


        public void BeginTrans()
        {
            _db.Database.BeginTransaction();
        }


        public void Commit()
        {
            _db.Database.CurrentTransaction?.Commit();
        }

        public void RollBack()
        {
            _db.Database.CurrentTransaction?.Rollback();
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
