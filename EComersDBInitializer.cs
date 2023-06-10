using EComersObjectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComersDBworkerLib
{
    public static class EComersDBInitializer
    {
        public static void EComersDBInitializ(EComersContext eComersContext, bool drop)
        {
            try
            {
                if (drop) eComersContext.Database.EnsureDeleted();
                eComersContext.Database.EnsureCreated();
                
                if (eComersContext.Description.Any())
                {
                    return ;
                }
                else
                {
                    List<Description> descriptionList = new List<Description>()
                    {
                        new Description()
                        {
                            DescriptionId = 1,
                            DescriptionText = "Administrator",
                            LangCod = "EN",
                            CreatedDate = DateTime.UtcNow,
                        },
                        new Description()
                        {
                            DescriptionId = 1,
                            DescriptionText = "Администратор",
                            LangCod = "RU",
                            CreatedDate = DateTime.UtcNow,
                        },
                        new Description()
                        {
                            DescriptionId = 2,
                            DescriptionText = "User",
                            LangCod = "EN",
                            CreatedDate = DateTime.UtcNow,
                        },
                        new Description()
                        {
                            DescriptionId = 2,
                            DescriptionText = "Пользователь",
                            LangCod = "RU",
                            CreatedDate = DateTime.UtcNow,
                        },
                        new Description()
                        {
                            DescriptionId = 3,
                            DescriptionText = "Activ",
                            LangCod = "EN",
                            CreatedDate = DateTime.UtcNow,
                        },
                        new Description()
                        {
                            DescriptionId = 3,
                            DescriptionText = "Активен",
                            LangCod = "RU",
                            CreatedDate = DateTime.UtcNow,
                        },
                        new Description()
                        {
                            DescriptionId = 4,
                            DescriptionText = "Removed",
                            LangCod = "EN",
                            CreatedDate = DateTime.UtcNow,
                        },
                        new Description()
                        {
                            DescriptionId = 4,
                            DescriptionText = "Удален",
                            LangCod = "RU",
                            CreatedDate = DateTime.UtcNow,
                        }
                        ,
                        new Description()
                        {
                            DescriptionId = 5,
                            DescriptionText = "Man",
                            LangCod = "EN",
                            CreatedDate = DateTime.UtcNow,
                        },
                        new Description()
                        {
                            DescriptionId = 6,
                            DescriptionText = "Woman",
                            LangCod = "EN",
                            CreatedDate = DateTime.UtcNow,
                        }
                    };

                    descriptionList.ForEach(description => eComersContext.Description.Add(description));
                    eComersContext.SaveChanges();

                    List<Gender> genderList = new List<Gender>()
                    {
                        new Gender()
                        {
                            GenderCod = "Men",
                            DescriptionId = 5,
                            DefaultGenderDescription = "Men"
                        },
                        new Gender()
                        {
                            GenderCod = "Women",
                            DescriptionId = 6,
                            DefaultGenderDescription = "Women"
                        },
                    };

                    genderList.ForEach(gender => eComersContext.Gender.Add(gender));
                    eComersContext.SaveChanges();

                    List<EmployeStatus> employeStatusesList = new List<EmployeStatus>()
                    {
                        new EmployeStatus()
                        {
                            Cod = "Work",
                            DefaultDescription = "Work"
                        },
                        new EmployeStatus()
                        {
                            Cod = "Fired",
                            DefaultDescription = "Fired"
                        }
                    };
                    employeStatusesList.ForEach(employeStatus => eComersContext.EmployeStatus.Add(employeStatus));
                    eComersContext.SaveChanges();

                    List<UserStatus> userStatusList = new List<UserStatus>()
                    {
                        new UserStatus()
                        {
                            Cod = "Act",
                            DescriptionId = 3,
                            DefaultDescription = "Activen"
                        },
                        new UserStatus()
                        {
                            Cod = "Rem",
                            DescriptionId = 4,
                            DefaultDescription = "Removed"
                        }
                    };

                    userStatusList.ForEach(useStatus => eComersContext.UsersStatus.Add(useStatus));
                    eComersContext.SaveChanges();

                    List<Role> rolesList = new List<Role>()
                    {
                        new Role()
                        {
                            Cod = "Admin",
                            DescriptionId = 1,
                            CreatedDate = DateTime.UtcNow
                            
                        },
                        new Role()
                        {
                            Cod = "User",
                            DescriptionId = 2,
                            CreatedDate = DateTime.UtcNow
                            
                        }
                    };

                    rolesList.ForEach(role => eComersContext.Role.Add(role));
                    eComersContext.SaveChanges();

                    List<User> userList = new List<User>()
                    {
                        new User()
                        {
                            Name = "Administrator",
                            Login = "root",
                            PasswordHash = "AA1ErrppyaCRFwPt23VjHGb9rVkSjG/wI0N+pOZOS9d0IleA6ZoHCjOLfoGmGUstzg==", //password1234
                            RoleId = 1,
                            StatusId = 1,
                            CreatedDate = DateTime.UtcNow,
                            CreatedUserId = 1,
                        },
                        new User()
                        {
                            Name = "User",
                            SecondName = "Test",
                            RoleId = 2,
                            StatusId = 1,
                            CreatedDate = DateTime.UtcNow,
                            CreatedUserId = 1,
                        }
                    };

                    userList.ForEach(user => eComersContext.User.Add(user));
                    eComersContext.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                string message = ex.Message;
            }
            
        }
    }
}
