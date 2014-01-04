using System;

namespace Barebone.Routing
{
	public class RouteAlreadyExistsException : Exception {
		public string RouteId { get; set; }

		public RouteAlreadyExistsException(string id)
			:base(string.Format("A route entry with the id {0} already exists in the routing table", id)){
			RouteId = id;
		}
	}
}
