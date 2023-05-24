using Microsoft.EntityFrameworkCore;
using Watch.Domain.Interfaces;
using Watch.Domain.Models;

namespace Watch.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        public UserRepository(WatchDbContext db) : base(db)
        {
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult<bool>(false);
        }

        public async Task<UserModel?> GetAsync(string id)
        {
            return await Task.FromResult<UserModel?>(_db.Users.FirstOrDefault(u => u.Id.Equals(id)));
        }

        public async Task<UserModel?> GetByUserNameAsync(string username)
        {
            return await Task.FromResult<UserModel?>(_db.Users.FirstOrDefault(u => u.UserName.Equals(username)));
        }

        private bool CheckRowVersion(byte[] db, byte[] updated)
        {
            if (db.Length != updated.Length)
            {
                return false;
            }
            int length = db.Length;
            for (int i = 0; i < length; i++)
            {
                if (db[i] != updated[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> SetActivityAsync(string id, bool activity)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            user.IsActive = activity;
            var res = await UpdateConcurrencyAsync(user);

            return res.Code == 200;
        }
        public async Task<ConcurrencyUpdateResultModel> UpdateConcurrencyAsync(UserModel entity)
        {
            try
            {
                var model = await _db.Users.FirstAsync(x => x.Id == entity.Id);
                model.FirstName = entity.FirstName;
                model.SecondName = entity.SecondName;
                model.LastName = entity.LastName;
                model.UserName = entity.UserName;

                if(model.Email != entity.Email)
                {
                    model.Email = entity.Email;
                    model.EmailConfirmed = false;
                }

                model.PhoneNumber = entity.PhoneNumber;

                if (!CheckRowVersion(model.RowVersion, entity.RowVersion))
                {
                    throw new DbUpdateConcurrencyException();
                }

                _db.Users.Update(model);
                await _db.SaveChangesAsync();

                return new ConcurrencyUpdateResultModel()
                {
                    Code = 200,
                    Message = "Ok"
                };
            }
            catch(DbUpdateConcurrencyException)
            {
                var model = await _db.Users.FindAsync(entity.Id);

                if (model == null)
                {
                    return new ConcurrencyUpdateResultModel()
                    {
                        Code = 404,
                        Message = "The entry was deleted"
                    };
                }

                return new ConcurrencyUpdateResultModel()
                {
                    Code = 409,
                    Message = "The entry was modified by another user"
                };
            }
            catch
            {
                return new ConcurrencyUpdateResultModel()
                {
                    Code = 500,
                    Message = "Internal Server Error"
                };
            }
            
        }

    }
}
