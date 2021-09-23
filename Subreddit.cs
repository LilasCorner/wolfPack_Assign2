/***************************************************************
CSCI 473      Assignment 2     Fall 2021

Programmer: Lila & Harshith

ZIDS: Z1838117 & Z1891464

Section: 01

Date Due: 9/23/21
***************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace wolfPack_Assign2
{
    public class Subreddit : IComparable<Subreddit>, IEnumerable
    {
        private Subreddit[] _sub;
        private readonly uint id;
        private string name;
        uint members;
        uint active;
        SortedSet<uint> subPostIDs = new SortedSet<uint>();




        //default constructor
        public Subreddit()
        {
            id = Form1.genId();
            Name = "";
            Members = 0;
            Active = 0;
            subPostIDs.Add(this.Id);
        }

        //alt constructor
        public Subreddit(params string[] tokens)
        {
            if (!Form1.UniqueId(Convert.ToUInt32(tokens[Form1.SUB_INDEX - 4])))
            {
                id = Form1.genId();
            }
            else
            {
                id = Convert.ToUInt32(tokens[Form1.SUB_INDEX - 4]);
            }

            Name = tokens[Form1.SUB_INDEX - 3];
            Members = Convert.ToUInt32(tokens[Form1.SUB_INDEX - 2]);
            Active = Convert.ToUInt32(tokens[Form1.SUB_INDEX - 1]);
        }

        //user created subreddit constructor
        public Subreddit(string name)
        {
            id = Form1.genId();
            Name = name;
            Members = 0;
            Active = 0;
        }


        public void addPost(uint _id)
        {
            subPostIDs.Add(_id);
            

        }

        //properties
        public uint Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }

            //perform validation of name has to be char >3 && <21 && cannot begin/end with space characters & cant have bad words
            set
            {
                
                if (Form1.validateName(value, 3, 21)) 
                {
                    name = value;
                }
            }
        }

        public uint Members
        {
            get { return members; }
            set { members = value; }
        }

        public uint Active
        {
            get { return active; }
            set { active = value; }
        }

        public SortedSet<uint> SubPostIDs
        {
            get { return subPostIDs; }
        }

        public override string ToString()
        {
            return Name;

        }

        public int CompareTo(object subred) //Sorting by alphanumeric name
        {
            if (subred == null) throw new ArgumentNullException();

            Subreddit rightOp = subred as Subreddit;

            if (rightOp != null) // Protect against a failed typecasting
                return Name.CompareTo(rightOp.Name); 
            else
                throw new ArgumentException("[Subreddit]:CompareTo argument is not a Subreddit");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public SubEnum GetEnumerator()
        {
            return new SubEnum(_sub);
        }

        public int CompareTo(Subreddit other)
        {
            if (other != null) // Protect against a failed typecasting
                return Name.CompareTo(other.Name);
            else
                throw new ArgumentException("[Subreddit]:CompareTo argument is not a Subreddit");
        }
    }

    public class SubEnum : IEnumerator
    {
        public Subreddit[] _sub;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public SubEnum(Subreddit[] list)
        {
            _sub = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _sub.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Subreddit Current
        {
            get
            {
                try
                {
                    return _sub[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }


}
