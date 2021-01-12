using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IssueTracking.Domain.Shared.Issues;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace IssueTracking.Domain.Issues
{
  public class Issue : AggregateRoot<Guid>
  {
    public Guid RepositoryId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public Guid? AssignedUserId { get; set; }
    public bool IsClosed { get; set; } = false;
    public IssueCloseReason? CloseReason { get; set; }
    public ICollection<IssueLabel> Labels { get; set; }
    public ICollection<Comment> Comments { get; private set; }

    public Issue(
            Guid id,
            Guid repositoryId,
            string title,
            string text = null,
            Guid? assignedUserId = null
            ) : base(id)
    {
      RepositoryId = repositoryId;
      Title = Check.NotNullOrWhiteSpace(title, nameof(title));

      Text = text;
      AssignedUserId = assignedUserId;

      Labels = new Collection<IssueLabel>();
      Comments = new Collection<Comment>();
    }

    private Issue() { /* for deserialization & ORMs */ }

    public void AddComment(Guid userId, string text)
    {
      Comments ??= new Collection<Comment>();
      Comments.Add(new Comment { IssueId = this.Id, Text = text, UserId = userId });
    }
  }
}