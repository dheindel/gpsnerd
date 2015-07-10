using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracker.Data;
using Tracker.Data.Entities;
using Tracker.Data.NHibernateLayer;

namespace NHibernateLayerTests
{
    [TestClass]
    public class NHibernateLayerTests
    {
        [TestMethod]
        [ExpectedException(typeof(NHibernate.Exceptions.GenericADOException))]
        public void Add_User_Fails_If_UserName_Is_Null()
        {
            User user = new User
                            {
                                FirstName = "Bob",
                                LastName = "Test",
                                UserName = null,
                                Email = "test@test.com"
                            };

            NHibernateHelper helper = new NHibernateHelper();
            using(UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<User> userRepo = new Repository<User>(unitOfWork.Session);

                userRepo.Add(user);

                unitOfWork.Commit();
            }
        }

        [TestMethod]
        public void Add_User_Fails_If_UserName_Not_Unique()
        {
            string userName = Guid.NewGuid().ToString();
            User user1 = new User
                            {
                                FirstName = "First",
                                LastName = "User1",
                                UserName = userName,
                                Email = "fuser1@test.com"
                            };
            User user2 = new User
            {
                FirstName = "Second",
                LastName = "User2",
                UserName = userName,
                Email = "suser2@test.com"
            };

            NHibernateHelper helper = new NHibernateHelper();

            // create
            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<User> userRepo = new Repository<User>(unitOfWork.Session);
                Assert.IsTrue(userRepo.Add(user1));
                unitOfWork.Commit();
            }
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
                {
                    Repository<User> userRepo = new Repository<User>(unitOfWork.Session);
                    Assert.IsTrue(userRepo.Add(user2));
                    unitOfWork.Commit();
                }
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                // expected this
            }
            finally
            {
                // delete
                using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
                {
                    Repository<User> userRepo = new Repository<User>(unitOfWork.Session);
                    Assert.IsTrue(userRepo.Delete(user1));
                    unitOfWork.Commit();
                }
            }
        }

        [TestMethod]
        public void Crud_User_Succeeds()
        {
            string userName = Guid.NewGuid().ToString();
            User user = new User
            {
                FirstName = "Bob",
                LastName = "Test",
                UserName = userName,
                Email = "test@test.com"
            };

            NHibernateHelper helper = new NHibernateHelper();

            // create
            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<User> userRepo = new Repository<User>(unitOfWork.Session);
                Assert.IsTrue(userRepo.Add(user));
                unitOfWork.Commit();
            }

            // read
            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<User> userRepo = new Repository<User>(unitOfWork.Session);
                User user1 = userRepo.FindBy(user.Id);
                Assert.IsTrue(AreEqual(user, user1));
                User user2 = userRepo.FindBy(c => c.UserName == userName);
                Assert.IsTrue(AreEqual(user, user2));
                unitOfWork.Commit();
            }

            // update
            const string newFirstName = "This is new";
            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<User> userRepo = new Repository<User>(unitOfWork.Session);
                User user1 = userRepo.FindBy(user.Id);
                user1.FirstName = newFirstName;
                Assert.IsTrue(userRepo.Update(user1));
                unitOfWork.Commit();
            }
            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<User> userRepo = new Repository<User>(unitOfWork.Session);
                User user1 = userRepo.FindBy(user.Id);
                Assert.IsTrue(user1.FirstName == newFirstName);
            }

            // delete
            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<User> userRepo = new Repository<User>(unitOfWork.Session);
                Assert.IsTrue(userRepo.Delete(user));
                unitOfWork.Commit();
            }
        }


        private static bool AreEqual(User user1, User user2)
        {
            if((user1==null && user2!=null) || (user1!=null && user2==null))
            {
                return false;
            }
            if (user1.Id != user2.Id) return false;
            if (user1.Email != user2.Email) return false;
            if (user1.FirstName != user2.FirstName) return false;
            if (user1.LastName != user2.LastName) return false;

            return true;
        }
    }
}
