using System;
using System.Collections.Generic;
using System.ServiceModel;
using PersistenceLayer.Dto;
using Server.Shared.Exceptions;

namespace Client
{
    internal class Program
    {
        private static void Main()
        {
            string activeLogin = null;
            const string usage = @"Usage:
help
login user password
logout
udata
myroles
ChangeMyPassword oldPasswd newPasswd
--- For Admin:
SetTimeout seconds
GetUsers
GetProjects
SetUser
RemoveUser
--- For Employee:
GetUserInfo
SetPassword login passwd
GetContracts
--- For manager:
SetContract
GetProjecsOfManager
---------------
exit

";
            Console.WriteLine("Employee: 1 --- Manager: 2 --- Admin: 3 --- Multi: 4");
            string ui = Console.ReadLine();
            bool isMultiUser = false;
            UIClient communicator;
            if(ui != null && ui.Equals("1"))
            {
                communicator = new EmployeeClient();
            }
            else if (ui != null && ui.Equals("2"))
            {
                communicator = new ManagerClient();
            }
            else if (ui != null && ui.Equals("3"))
            {
                communicator = new AdminClient();
            }
            else
            {
                communicator = new MultiClient();
                isMultiUser = true;
            }


            Console.WriteLine("Communication established.");

            while (true)
            {
                Console.Write(">>>");
                var command = Console.ReadLine();
                if (command == null) continue;
                var comms = command.Split(' ');

                if (comms[0].Equals("help"))
                    Console.Write(usage);

                else if (comms[0].Equals("login"))
                {
                    try
                    {
                        if (isMultiUser)
                        {
                            var roles = communicator.MultiLogin(comms[1], comms[2]);
                            foreach (var role in roles)
                            {
                                Console.WriteLine(role);
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine(communicator.Login(comms[1], comms[2])
                                                  ? "Loging successful"
                                                  : "Incorrect login or password");
                        }
                        activeLogin = comms[1];
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                else if (comms[0].Equals("logout"))
                {
                    try
                    {
                        Console.WriteLine(communicator.Logout() ? "Logout successful" : "Some error...");
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }

                else if (comms[0].Equals("udata"))
                {
                    try
                    {
                        /*User u = communicator.GetLoggedUserData();
                        Console.WriteLine(u != null
                                              ? u.PublicUserInfo.Login
                                                + ", " + u.PublicUserInfo.Name + ", " + u.PublicUserInfo.Surname
                                              : "error");*/
                        Console.WriteLine("Not supported");
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }

                else if (comms[0].Equals("myroles"))
                {
                    try
                    {
                        /*User u = communicator.GetLoggedUserData();
                        if (u != null)
                            foreach (var r in u.Roles)
                                Console.WriteLine(r);*/
                        Console.WriteLine("Not implemented");
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }

                else if (comms[0].Equals("GetUsers"))
                {
                    try
                    {
                        foreach(var user in communicator.GetUsers())
                        {
                            Console.WriteLine(user.PublicUserInfo.Name + "   " + user.PublicUserInfo.Surname);
                        }
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                        Console.WriteLine(e.Message);
                    }
                }
                else if (comms[0].Equals("AddEmployee"))
                {
                    try
                    {
                        User user = new User
                        {
                            PasswordHash = "c81e728d9d4c2f636f067f89cc14862c".ToUpper(), // "2"
                            PublicUserInfo = new PublicUserInfo { Login = "anna", Name = "Anna", Surname = "Nowak" },
                            Roles = new List<Role> { Role.NormalUser }
                        };
                        communicator.SetUser(user);
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                        Console.WriteLine(e.Message);
                    }
                }
                else if (comms[0].Equals("GetProjects"))
                {
                    try
                    {
                        communicator.GetProjects();
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }
                else if (comms[0].Equals("SetUser"))
                {
                    try
                    {
                        communicator.SetUser(new User());
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }
                else if (comms[0].Equals("RemoveUser"))
                {
                    try
                    {
                        communicator.RemoveUser(new User());
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }
                else if (comms[0].Equals("GetUserInfo"))
                {
                    try
                    {
                        communicator.GetUserInfo(activeLogin);
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }
                else if (comms[0].Equals("SetPassword"))
                {
                    try
                    {
                        communicator.SetPassword(comms[1], comms[2]);
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }
                else if (comms[0].Equals("ChangeMyPassword"))
                {
                    try
                    {
                        communicator.ChangeMyPassword(comms[1], comms[2]);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<IncorrectOldPassword> e)
                    {
                        Console.WriteLine("*** IncorrectOldPassword");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }
                else if (comms[0].Equals("GetContracts"))
                {
                    try
                    {
                        communicator.GetContracts(new User { PublicUserInfo = new PublicUserInfo { Login = activeLogin } });
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }
                else if (comms[0].Equals("SetContract"))
                {
                    try
                    {
                        communicator.SetContract(new Contract());
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }
                else if (comms[0].Equals("GetProjecsOfManager"))
                {
                    try
                    {
                        communicator.GetProjecsOfManager(new User { PublicUserInfo = new PublicUserInfo { Login = activeLogin } });
                    }
                    catch (FaultException<PermissionDeniedForUser> e)
                    {
                        Console.WriteLine("*** PermissionDeniedForUser:");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (FaultException<NoActiveSession> e)
                    {
                        Console.WriteLine("*** NoActiveSession");
                        Console.WriteLine(e.Detail.Message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }
                else if (comms[0].Equals("exit"))
                    break;

                else
                    Console.Write(usage);
            }
        }
    }
}