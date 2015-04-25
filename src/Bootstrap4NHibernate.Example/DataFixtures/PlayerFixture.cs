/*The MIT License (MIT)

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
using Bootstrap4NHibernate.Data;
using Bootstrap4NHibernate.Example.Model;

namespace Bootstrap4NHibernate.Example.DataFixtures
{
    public class PlayerFixture : DataFixture
    {
        public readonly Player JoeBloggs = new Player {Name = "Joe Bloggs"};
        public readonly Player FredSmith = new Player { Name = "Fred Smith" };
        public readonly Player DimTim = new Player { Name = "Dim Tim" };
        public readonly Player FastEddy = new Player { Name = "Fast Eddy" };
        public readonly Player SpongeBob = new Player { Name = "Sponge Bob" };
        public readonly Player FatherTed = new Player { Name = "Father Ted" };

        public override Type[] Dependencies
        {
            get { return new[] { typeof(TeamFixture) }; }
        }

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var teamFixture = fixtureContainer.Get<TeamFixture>();

            teamFixture.Donkeys.AddPlayer(FatherTed);
            teamFixture.Donkeys.AddPlayer(SpongeBob);
            teamFixture.Falcons.AddPlayer(FastEddy);
            teamFixture.Falcons.AddPlayer(DimTim);
            teamFixture.Jets.AddPlayer(FredSmith);
            teamFixture.Jets.AddPlayer(JoeBloggs);

            return new[]
            {
                JoeBloggs,
                FredSmith,
                DimTim,
                FastEddy,
                SpongeBob,
                FatherTed
            };
        }
    }
}
