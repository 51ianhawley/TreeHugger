using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeHugger.Models;

public class Comment : INotifyPropertyChanged
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
            OnPropertyChanged(nameof(userName));
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
            OnPropertyChanged(nameof(commentText));
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
            OnPropertyChanged(nameof(timePosted));
        }
    }

    public Comment(String commentText)
    {
        this.userName = MauiProgram.BusinessLogic.Username == null ? "Guest" : MauiProgram.BusinessLogic.Username;
        this.commentText = commentText;
        this.timePosted = DateTime.Now;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
