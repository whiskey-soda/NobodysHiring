using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CheckEmails : Activity
{
    Inbox inbox;

    protected override void Start()
    {
        base.Start();

        inbox = Inbox.Instance;
    }

    public override void DoActivity(float duration)
    {
        base.DoActivity(duration);


    }


}
