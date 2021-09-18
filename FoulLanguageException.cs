/***************************************************************
CSCI 473      Assignment 1     Fall 2021

Programmer: Lila & Harshith

ZIDS: Z1838117 & Z1891464

Section: 01

Date Due: 9/9/21
***************************************************************/


using System;
using System.Runtime.Serialization;

namespace wolfPack_Assign2
{
    [Serializable]
    internal class FoulLanguageException : Exception
    {

        public FoulLanguageException()
        {

        }

        public FoulLanguageException(string message)
            : base(message) { }

        public FoulLanguageException(string message, Exception inner)
            : base(message, inner) { }


        public override string ToString()
        {
            return "\nA bad word was found within the contents of this text. Shame on you.";
        }
    }
}