using System;
using System.Collections.Generic;
using System.ServiceModel;
using Client;
using Server.Shared.Exceptions;
using PersistenceLayer.Dto;

namespace AdminConsole
{
    class Program
    {
        private static void Main()
        {
            var communicator = new AdminClient();
           
        
            string activeLogin = null;
            const string usage = @"Usage:
help
login user password
logout

GetEmployeesDescriptions: ged
GetUsers: gu
add employee : ae <user> <passwd> <first name> <second name>
add manager: am <user> <passwd>
exit

";

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
                        Console.WriteLine(communicator.Login(comms[1], comms[2])
                                              ? "Loging successful"
                                              : "Incorrect login or password");
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
                   
                else if (comms[0].Equals("ged"))
                {
                    try
                    {
                        EmployeeDescription[] list = communicator.GetEmployeesDescriptions();
                        foreach (var employeeDescription in list)
                        {
                            Console.WriteLine(employeeDescription);
                            
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
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }

                else if (comms[0].Equals("gu"))
                {
                    try
                    {
                        User[] list = communicator.GetUsers();
                        foreach (var employee in list)
                        {
                            Console.WriteLine(employee);

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
                    catch (Exception)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                    }
                }


                else if (comms[0].Equals("ae"))
                {
                    try
                    {
                        var employee = new User
                            {
                                PasswordHash = PasswordHasher.ComputeHash(comms[2]),
                                PublicUserInfo = new PublicUserInfo() {Login = comms[1], Name = comms[3], Surname = comms[4]},
                                Roles = new List<Role> {Role.NormalUser}
                            };

                        communicator.SetUser(employee);
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
