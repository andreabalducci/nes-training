using System;
using System.Collections.Generic;
using System.Text;
using Bookings.Domain.BookingContenxt;
using Bookings.Domain.BookingContenxt.Events;
using Machine.Specifications;

// ReSharper disable InconsistentNaming

namespace Bookings.Tests.DomainTests
{
    [Subject(typeof(Resource))]
    public class when_a_resource_is_created
    {
        static readonly Guid _id = new Guid("4534C386-5284-4203-9AA3-87B60A172764");
        static Resource resource;

        Establish context = () =>
        {
            resource = null;
        };

        Because of = () =>
        {
            // discuss: "new Resource" == "Create Resource" 
            resource = new Resource(_id, "MacBook Pro 13\"");
        };

        // state transitions
        It identity_should_be_set = () => 
            resource.Id.ShouldBeLike(_id);
        
        It description_should_be_set = () => 
            resource.Description.ShouldBeLike("MacBook Pro 13\"");
        
        It resource_should_not_be_available = () => 
            resource.Available.ShouldBeFalse();

        // events
        It resource_should_had_raised_created_event = () =>
            resource.ShouldHadRaised<ResourceCreated>();

        It created_event_should_have_id = () =>
            resource.LastEventOfType<ResourceCreated>().Id.ShouldBeLike(_id);

        It created_event_should_have_description = () =>
            resource.LastEventOfType<ResourceCreated>().Description.ShouldNotBeNull();
    }
}
