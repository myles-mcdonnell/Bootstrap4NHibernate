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
using Bootstrap4NHibernate.Example.Model;

namespace Bootstrap4NHibernate.Example.DataFixtures
{
    public class StadiumFixture : Data.DataFixture
    {
        public readonly Stadium MegaBowl = new Stadium {Name = "Mega Bowl"};
        public readonly Stadium MuddyField = new Stadium {Name = "Muddy Field"};

        public override Type[] Dependencies
        {
            get { return new []{typeof(TeamFixture)}; }
        }

        public override object[] GetEntities(Data.IFixtureContainer fixtureContainer)
        {
            var teamFixture = fixtureContainer.Get<TeamFixture>();

            MegaBowl.AddTeam(teamFixture.Jets);
            MuddyField.AddTeam(teamFixture.Falcons);
            MuddyField.AddTeam(teamFixture.Donkeys);

            return new []
            {
                MegaBowl,
                MuddyField
            };
        }
    }
}
