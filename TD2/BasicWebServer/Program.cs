﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace BasicServerHTTPlistener { 


    public class MyMethods
    {
        public static string add(String param1, String param2)
        {
            try
            {
                int n1 = Int32.Parse(param1);
                int n2 = Int32.Parse(param2);
                return (n1 + n2).ToString();
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
        }

        public static string substract(String param1, String param2)
        {
            try
            {
                int n1 = Int32.Parse(param1);
                int n2 = Int32.Parse(param2);
                return (n1 - n2).ToString();
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
        }

        public static string multiply(String param1, String param2)
        {
            try
            {
                int n1 = Int32.Parse(param1);
                int n2 = Int32.Parse(param2);
                return (n1 * n2).ToString();
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
        }

        public static string incr(String param1)
        {
            try
            {
                int n1 = Int32.Parse(param1);
                return (n1 + 1).ToString();
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
        }
    }


    internal class Program
    {
        private static void Main(string[] args)
        {

            //if HttpListener is not supported by the Framework
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("A more recent Windows version is required to use the HttpListener class.");
                return;
            }


            // Create a listener.
            HttpListener listener = new HttpListener();

            // Add the prefixes.
            if (args.Length != 0)
            {
                foreach (string s in args)
                {
                    listener.Prefixes.Add(s);
                    // don't forget to authorize access to the TCP/IP addresses localhost:xxxx and localhost:yyyy 
                    // with netsh http add urlacl url=http://localhost:xxxx/ user="Tout le monde"
                    // and netsh http add urlacl url=http://localhost:yyyy/ user="Tout le monde"
                    // user="Tout le monde" is language dependent, use user=Everyone in english 

                }
            }
            else
            {
                Console.WriteLine("Syntax error: the call must contain at least one web server url as argument");
            }
            listener.Start();

            // get args 
            foreach (string s in args)
            {
                Console.WriteLine("Listening for connections on " + s);
            }

            // Trap Ctrl-C on console to exit 
            Console.CancelKeyPress += delegate
            {
                // call methods to close socket and exit
                listener.Stop();
                listener.Close();
                Environment.Exit(0);
            };


            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }

                // get url 
                Console.WriteLine($"Received request for {request.Url}");

                //get url protocol
                Console.WriteLine(request.Url.Scheme);
                //get user in url
                Console.WriteLine(request.Url.UserInfo);
                //get host in url
                Console.WriteLine(request.Url.Host);
                //get port in url
                Console.WriteLine(request.Url.Port);
                //get path in url 
                Console.WriteLine(request.Url.LocalPath);

                // parse path in url 
                foreach (string str in request.Url.Segments)
                {
                    Console.WriteLine(str);
                }

                //get params un url. After ? and between &

                Console.WriteLine(request.Url.Query);

                //parse params in url
                Console.WriteLine("param1 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param1"));
                Console.WriteLine("param2 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param2"));
                Console.WriteLine("param3 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param3"));
                Console.WriteLine("param4 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param4"));

                string[] parameters =
                {
                    HttpUtility.ParseQueryString(request.Url.Query).Get("param1"),
                    HttpUtility.ParseQueryString(request.Url.Query).Get("param2"),
                    HttpUtility.ParseQueryString(request.Url.Query).Get("param3")
                };

                //
                Console.WriteLine(documentContents);

                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                string responseString = "";

                if (request.Url.Segments.Length >= 2)
                {
                    switch (request.Url.Segments[1])
                    {
                        case "exercice1/":
                            // Example of url to use : http://localhost:8080/exercice1/substract?param1=12&param2=5
                            string htmlResponse = "";

                            if (request.Url.Segments.Length >= 3)
                            {
                                Type methodsType = typeof(MyMethods);
                                MethodInfo method = methodsType.GetMethod(request.Url.Segments[2]);

                                if (method == null)
                                {
                                    htmlResponse = "You have to give a defined method in parameter (add, substract, multiply)";
                                }
                                else
                                {
                                    try
                                    {
                                        string result = (string)methodsType.GetMethod(request.Url.Segments[2]).Invoke(null, new object[] { parameters[0], parameters[1] });
                                        htmlResponse = $"The result of the method is {result}";
                                    }
                                    catch (TargetInvocationException)
                                    {
                                        htmlResponse = "Error of format";
                                    }
                                }
                                
                            }
                            else
                            {
                                htmlResponse = "3 parameters should be given";
                            }
                            responseString = $"<!DOCTYPE html><html><body>{htmlResponse}</body></html>";
                            break;

                        case "exercice2/":
                            // Example of url : http://localhost:8080/exercice2/multiply?param1=multiply&param2=12&param3=5
                            ProcessStartInfo start = new ProcessStartInfo();
                            start.FileName = "python";
                            start.Arguments = $"../../{request.Url.Segments[2]}.py ";

                            foreach (string param in parameters)
                            {
                                start.Arguments += ((param == null || param.Equals("")) ? "undefined " : param + " ");
                            }

                            Console.Write("Method arguments: " + start.Arguments);

                            start.UseShellExecute = false;
                            start.RedirectStandardOutput = true;

                            using (Process process = Process.Start(start))
                            {
                                using (StreamReader reader = process.StandardOutput)
                                {
                                    string result = reader.ReadToEnd();
                                    responseString = result;
                                }
                            }

                            break;

                        case "exercice3/":

                            if (request.Url.Segments.Length == 2)
                            {
                                Type methodsType = typeof(MyMethods);
                                MethodInfo method = methodsType.GetMethod(request.Url.Segments[2]);

                                if (method == null)
                                {
                                    htmlResponse = "You have to give a defined method in parameter (add, substract, multiply, incr)";
                                }
                                else
                                {
                                    try
                                    {
                                        string result = (string)methodsType.GetMethod(request.Url.Segments[2]).Invoke(null, new object[] { parameters[0], parameters[1] });
                                        htmlResponse = $"The result of the method is {result}";
                                        responseString = $"<!DOCTYPE html><html><body>{htmlResponse}</body></html>";
                                    }
                                    catch (TargetInvocationException)
                                    {
                                        htmlResponse = "Error of format";
                                    }
                                }


                            }
                            else
                            {
                                htmlResponse = "3 parameters should be given";
                            }
                            break;

                        default:
                            responseString = $"<!DOCTYPE html><html><body>Invalid path used</body></html>";
                            break;
                    }

                }
                else
                {
                    responseString = $"<!DOCTYPE html><html><body>Invalid path used</body></html>";
                }

                // Construct a response.
                /*Type type = typeof(MyReflectionClass);
                MethodInfo method = type.GetMethod("MyMethod");
                MyReflectionClass c = new MyReflectionClass();
                string responseString = (string)method.Invoke(c, null);
                Console.WriteLine(response);
                Console.ReadLine();*/

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            }
            // Httplistener neither stop ... But Ctrl-C do that ...
            // listener.Stop();
        }


    }
   
}