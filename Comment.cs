/***************************************************************
CSCI 473      Assignment 1     Fall 2021

Programmer: Lila & Harshith

ZIDS: Z1838117 & Z1891464

Section: 01

Date Due: 9/9/21
***************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace wolfPack_Assign2
{
    public class Comment : IEnumerable, IComparable
    {
        public Comment[] _comment;
        readonly uint id; //— a unique identifier for this Comment.
        readonly uint authorID; //— corresponding ID of the User that posted this.
        string content;//— screen for vulgarities.
        readonly uint parentID; //— corresponding ID of either the Post or Comment this replies to.
        uint upVotes;
        uint downVotes;
        uint indent;
        readonly DateTime timeStamp = new DateTime();
        List<Comment> commentReplies = new List<Comment>();
        
        //default constructor
        public Comment()
        {
            id = Form1.genId();
            authorID = Form1.genId();
            Content = "";
            parentID = Form1.genId();
            UpVotes = 1;
            DownVotes = 0;
            timeStamp = DateTime.Now;
            indent = 0;
        }


        //alt constructor
        public Comment(params string[] tokens)
        {

            id = Convert.ToUInt32(tokens[Form1.COM_INDEX - 12]);
            


            authorID = Convert.ToUInt32(tokens[Form1.COM_INDEX - 11]);
            

            Content = tokens[Form1.COM_INDEX - 10];

  
            
            parentID = Convert.ToUInt32(tokens[Form1.COM_INDEX - 9]);

            UpVotes = Convert.ToUInt32(tokens[Form1.COM_INDEX - 8]);
            DownVotes = Convert.ToUInt32(tokens[Form1.COM_INDEX - 7]);
            timeStamp = new DateTime(Convert.ToInt32(tokens[Form1.COM_INDEX - 6]), Convert.ToInt32(tokens[Form1.COM_INDEX - 5]), Convert.ToInt32(tokens[Form1.COM_INDEX - 4]), Convert.ToInt32(tokens[Form1.COM_INDEX - 3]), Convert.ToInt32(tokens[Form1.COM_INDEX - 2]), Convert.ToInt32(tokens[Form1.COM_INDEX - 1]));

        }

        //new comment constructor

        public Comment(string _content, uint _authorId, uint _parentId)
        {

            id = Form1.genId();

            if (!Form1.UniqueId(_authorId))
            {
                authorID = Form1.genId();
            }
            else
            {
                authorID = _authorId;
            }

            Content = _content;


            if (!Form1.UniqueId(_parentId))
            {
                parentID = Form1.genId();
            }
            else
            {
                parentID = _parentId;
            }

            UpVotes = 1;
            DownVotes = 0;
            timeStamp = DateTime.Now;
        }

        //adds new comment to commentReplies List
        public void addComment(Comment newCom)
        {
            commentReplies.Add(newCom);
        }

        //properties below
        public uint Id
        {
            get { return id; }

        }

        public uint AuthorId
        {
            get { return authorID; }

        }

        public List<Comment> CommentReplies
        {
            get { return commentReplies; }
        }

        public string Content
        {
            get { return content; }
            set
            {
                if (Form1.findBadWords(value))
                {
                    content = "";
                    throw new FoulLanguageException(); 
                }
                else
                {
                    content = value;
                }
            }
        }

        public uint Indent
        {
            get { return indent; }
            set { indent = value; }
        }

        public uint ParentId
        {
            get { return parentID; }
        }

        public uint UpVotes
        {
            get { return upVotes; }
            set { upVotes = value; }
        }

        public uint DownVotes
        {
            get { return downVotes; }
            set { downVotes = value; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        public int Score => Convert.ToInt32(UpVotes) - Convert.ToInt32(DownVotes);

        public int CompareTo(object com)
        {
            if (com == null) throw new ArgumentNullException();

            Comment rightOp = com as Comment;

            if (rightOp != null) // Protect against a failed typecasting
                return Score.CompareTo(rightOp.Score);
            else
                throw new ArgumentException("[Comment]:CompareTo argument is not a Comment");
        }

        // Implementation for the GetEnumerator method.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public CommentEnum GetEnumerator()
        {
            return new CommentEnum(_comment);
        }

        public override string ToString()
        {
            return "<"+Id+">" + " (" + Score + ") " + Content + " - " + Form1.usersMap[AuthorId].Name + " |" + TimeStamp + "| \n\n";

        }

    }

    // When you implement IEnumerable, you must also implement IEnumerator.
    public class CommentEnum : IEnumerator
    {
        public Comment[] _comment;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public CommentEnum(Comment[] list)
        {
            _comment = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _comment.Length);
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

        public Comment Current
        {
            get
            {
                try
                {
                    return _comment[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
