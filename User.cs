/***************************************************************
CSCI 473      Assignment 1     Fall 2021

wolfPack_Assign2mer: Lila & Harshith

ZIDS: Z1838117 & Z1891464

Section: 01

Date Due: 9/9/21
***************************************************************/


using System;
using System.Collections.Generic;
using System.Text;

namespace wolfPack_Assign2
{
    public class User 
    {

        private readonly uint id;
        private string name;
        private uint userType=0;
        private int postScore;
        private int commentScore;
        private string passHash;
        List<String> modSubs = new List<String>();



        enum userTypes
        {
            User,
            Mod,
            Admin
        }


        //default constructor
        public User()
        {
            id = Form1.genId();
            Name = "";
            PostScore = 0;
            CommentScore = 0;
        }

        public User(string _name)
        {
            id = Form1.genId();
            Name = _name;
            PostScore = 0;
            CommentScore = 0;
        }

        //this constructor assumes all data read from txt files is good, no need to perform validation
        public User(params string[] tokens)
        {

            if(tokens.Length == Form1.USER_INDEX) //no moderating subs to handle
            {
                if (!Form1.UniqueId(Convert.ToUInt32(tokens[Form1.USER_INDEX - 6])))
                {
                    id = Form1.genId();
                }
                else
                {
                    id = Convert.ToUInt32(tokens[Form1.USER_INDEX - 6]);
                }

                UserType = Convert.ToUInt32(tokens[Form1.USER_INDEX - 5]);

                Name = tokens[Form1.USER_INDEX - 4];

                passHash = tokens[Form1.USER_INDEX - 3];

                PostScore = Convert.ToInt32(tokens[Form1.USER_INDEX - 2]);
                CommentScore = Convert.ToInt32(tokens[Form1.USER_INDEX - 1]);
                //no moderating subs passed if tokens.length == user_index
            
            }
            else
            {
                if (!Form1.UniqueId(Convert.ToUInt32(tokens[tokens.Length-tokens.Length])))
                {
                    id = Form1.genId();
                }
                else
                {
                    id = Convert.ToUInt32(tokens[tokens.Length - tokens.Length]);
                }

                UserType = Convert.ToUInt32(tokens[tokens.Length - tokens.Length+1]);

                Name = tokens[tokens.Length - tokens.Length + 2];

                passHash = tokens[tokens.Length - tokens.Length + 3];

                PostScore = Convert.ToInt32(tokens[tokens.Length - tokens.Length + 4]);
                CommentScore = Convert.ToInt32(tokens[tokens.Length - tokens.Length + 5]);

                for(int i = 0; i < tokens.Length - Form1.USER_INDEX; i++)
                {
                    modSubs.Add(tokens[Form1.USER_INDEX + i]);
                }
            }

        }


        //properties
        public uint Id
        {
            get { return id; }

        }

        public uint UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        public string Name
        {
            get { return name; }

            //perform validation of name has to be char >5 && <21 && cannot begin/end with space characters
            set
            {
                if (Form1.validateName(value, 5, 21))
                {
                    name = value;
                }
            }
        }

        public string PassHash
        {
            get { return passHash; }

        }
        public int PostScore
        {
            get { return postScore; }

            set { postScore = value; }
        }

        public int CommentScore
        {
            get { return commentScore; }

            set { commentScore = value; }
        }

        public int TotalScore => commentScore + postScore;



        public override string ToString()
        {
            string userType = "";

            if (UserType == 1)
            {
                userType = "(M)";
            }
            else if(UserType == 2)
            {
                userType = "(A)";
            }

            return String.Format("{0, -10}  {1}   ({2}/{3})", Name, userType, PostScore, CommentScore);

        }
    }

}
