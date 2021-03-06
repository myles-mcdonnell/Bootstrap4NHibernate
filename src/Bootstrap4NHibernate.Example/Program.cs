﻿/*The MIT License (MIT)

Copyright (c) 2015 Shiftkey Software

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System;
using System.Reflection;
using Bootstrap4NHibernate.Example.Model;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Linq;

namespace Bootstrap4NHibernate.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //var dbConf = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(c => c
                    //.Database("Bootstrap4NHibernate")
                    //.Host("172.17.8.101")
                    //.Port(5432)
                    //.Username("postgres")
                    //.Password("postgres"));

                var dbConf =
                    MsSqlConfiguration.MsSql2012.ConnectionString(
                        @"server=(local)\MSSQL2012DEV;database=Bootstrap4NHibernate;uid=b4n;pwd=b4n123");

                var database = new Database(dbConf, Assembly.GetExecutingAssembly(), null, true);

                database.Populate(Assembly.GetExecutingAssembly());

                var sessionFactory = Fluently.Configure()
                    .Database(() => dbConf)
                    .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                    .BuildSessionFactory();

                using (var session = sessionFactory.OpenSession())
                {
                    foreach (var team in session.Query<Team>())
                    {
                        Console.WriteLine("Team: " + team.Name);
                        Console.WriteLine("Stadium: " + team.Stadium.Name);
                        Console.WriteLine("Players: ");

                        foreach (var player in team.Players)
                            Console.WriteLine(player.Name);

                        Console.WriteLine();
                    }
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
