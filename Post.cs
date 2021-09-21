/***************************************************************
CSCI 473      Assignment 1     Fall 2021

Programmer: Lila & Harshith

ZIDS: Z1838117 & Z1891464

Section: 01

Date Due: 9/9/21
***************************************************************/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace wolfPack_Assign2
{
    public class Post : IEnumerable,IComparable
    {
        private bool locked; 
        private readonly uint id;
        private string title;
        private uint authorId;
        private string postContent;
        private readonly uint subHome;
        private uint upVotes;
        private uint downVotes;
        private uint weight;
        private readonly DateTime timeStamp = new DateTime();

        
        private Post[] _post;

        private List<Comment> postComments = new List<Comment>(); 
        
        public Post()
        {
            locked = false;
            id = Form1.genId();
            Title = "";
            authorId = Form1.genId();
            PostContent = "";
            subHome = Form1.genId();
            timeStamp = DateTime.Now;
            UpVotes = 1;
            DownVotes = 0;
            Weight = 0;
        }

        public Post(string _title, uint _authorId, string _postContent, uint _subHome)
        {
            locked = false;
            id = Form1.genId();
            Title = _title;
            authorId = _authorId;
            PostContent = _postContent;
            subHome = _subHome;
            timeStamp = DateTime.Now;
            UpVotes = 1;
            DownVotes = 0;
            Weight = 0;

        }

        public Post(params string[] tokens)
        {
            if (Convert.ToInt32(tokens[Form1.POST_INDEX - 15]) == 1)
            {
                locked = true;
            }
            else
            {
                locked = false;
            }
            if (!Form1.UniqueId(Convert.ToUInt32(tokens[Form1.POST_INDEX - 14])))
            {
                id = Form1.genId();
            }
            else
            {
                id = Convert.ToUInt32(tokens[Form1.POST_INDEX - 14]);
            }


            authorId = Convert.ToUInt32(tokens[Form1.POST_INDEX - 13]);
            

            title = tokens[Form1.POST_INDEX - 12];


            PostContent = tokens[Form1.POST_INDEX - 11];


            subHome = Convert.ToUInt32(tokens[Form1.POST_INDEX - 10]);
            

            UpVotes = Convert.ToUInt32(tokens[Form1.POST_INDEX - 9]);

            DownVotes = Convert.ToUInt32(tokens[Form1.POST_INDEX - 8]);


            Weight = Convert.ToUInt32(tokens[Form1.POST_INDEX - 7]);

            timeStamp = new DateTime(Convert.ToInt32(tokens[Form1.POST_INDEX - 6]), Convert.ToInt32(tokens[Form1.POST_INDEX - 5]), Convert.ToInt32(tokens[Form1.POST_INDEX - 4]), Convert.ToInt32(tokens[Form1.POST_INDEX - 3]), Convert.ToInt32(tokens[Form1.POST_INDEX - 2]), Convert.ToInt32(tokens[Form1.POST_INDEX - 1]));



        }
        public uint Id
        {
            get { return id; }
        }

        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        public double Score => UpVotes - DownVotes;
        public string Title
        {
            get { return title; }
            set
            {
                if (Form1.validateName(value, 1, 100) && !Form1.findBadWords(value))
                {
                    title = value;
                }
                else
                {
                    title = "";
                }
            }
        }
        
        public List<Comment> PostComments
        {
            get { return postComments; }
        }

        public uint AuthorId
        {
            get { return authorId; }


        }
        // public string PostContent

        //properties

        public void addComment(Comment newCom)
        {
            postComments.Add(newCom);
        }

        public string PostContent 
        {

            get { return postContent; }
            set
            {
                if (Form1.validateName(value, 1, 1000) && !Form1.findBadWords(value))
                {
                    postContent = value;
                }
                else
                {
                    postContent = "";
                }
            }
        }


        public uint SubHome
        {
            get { return subHome; }
        }

        public uint UpVotes
        {
            get { return upVotes; }
            set
            {
                upVotes = value;
            }
        }

        public uint DownVotes
        {
            get { return downVotes; }
            set
            {
                downVotes = value;
            }
        }

        public uint Weight
        {
            get { return weight; }
            set
            {
                weight = value;
            }
        }
        public double PostRating
        {
            get
            {
                if (Weight == 0)
                {
                    return Score;
                }
                else if (Weight == 1)
                {
                    return Score * .66;
                }
                
                return 0.0;
                
            }
        }

       IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public PostEnum GetEnumerator()
        {
            return new PostEnum(_post);
        }

        public int CompareTo(Object alpha)
        {
            if (alpha == null) throw new ArgumentNullException(); // Always... check for null values

            Post rightOp = alpha as Post;

            if (rightOp != null) // Protect against a failed typecasting
                return PostRating.CompareTo(rightOp.PostRating); 
            else
                throw new ArgumentException("[Post]:CompareTo argument is not a Post");
        }
        public override string ToString()
        {
            return "<" + Id + ">" + " [" + SubHome + "] " + "(" + Score + ") " + Title + " : " + PostContent + " - " + " |"+ timeStamp +"| \n";
        }


        public string ToStringShort()
        {
            string shortTitle = "";
            if (Title.Length > 35)
            {
                shortTitle = Title.Substring(0, 35) + "...";
            }
            else
            {
                shortTitle = Title;
            }

            return "<" + Id + ">" + " [" + SubHome + "] " + "(" + Score + ") " + shortTitle  + " - " + " |" + timeStamp + "| \n";
        }



    }
    public class PostEnum : IEnumerator
    {
        public Post[] _post;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public PostEnum(Post[] list)
        {
            _post = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _post.Length);
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

        public Post Current
        {
            get
            {
                try
                {
                    return _post[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

}



    





