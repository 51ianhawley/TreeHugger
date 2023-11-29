using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeHugger.Models;

public class Comment
{
    String userName;
    String commentText;
    DateTime timePosted;

    public String Username { get
        {
            return userName;
        }
        set
        {
            userName = value;
        }
    }
    public String CommentText
    {
        get
        {
            return commentText;
        }
        set
        {
            commentText = value;
        }
    }
    public DateTime TimePosted
    {
        get {
            return timePosted; 
        }
        set { 
            timePosted = value; 
        }
    }

    public Comment(String commentText)
    {
        this.userName = MauiProgram.BusinessLogic.Username == null ? "Guest" : MauiProgram.BusinessLogic.Username;
        this.commentText = commentText;
        this.timePosted = DateTime.Now;
    }
}
