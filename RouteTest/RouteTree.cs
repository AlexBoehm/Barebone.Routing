using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteTest
{
	public class RouteTree{
		StaticNode _root = new StaticNode(string.Empty);

		public void Add(params Route[] routes){
			foreach (var route in routes) {
				Add(route);
			}
		}

		public void Add(Route route){
			var segments = new Stack<Segment>(route.Segments.Segments.Reverse());
			AddInternal(route, segments, _root);
		}

		void AddInternal(Route route, Stack<Segment> segments, StaticNode node){
			if (segments.Count == 0) {
				node.Leaves.Add(route);
				return;
			}

			var segment = segments.Pop();
			if (segment is StaticSegment) {
				StaticNode subNode;
				node.StaticSegments.Add(segment, out subNode);
				AddInternal(route, segments, subNode);
			} else {
				node.Leaves.Add(route);
			}
		}

		public List<Route> GetCandidates(string path){
			var segments = path.Substring(1, path.Length-1).Split('/');
			var result = new List<Route>();
			FindCandidates(segments, 0, _root, result);
			return result;
		}

		private void FindCandidates(string[] segments, int currentSegment, StaticNode node, List<Route> result){
			var pathIsNotTooLong = currentSegment < segments.Length;
			if (pathIsNotTooLong){
				var nextSegment = segments[currentSegment];

				var subNode = node.StaticSegments.Get(nextSegment);
				if (subNode != null) {
					FindCandidates(segments, currentSegment + 1, subNode, result);
				}
			}

			foreach (var route in node.Leaves) {
				if (route.Segments.Segments.Length == segments.Length) {
					result.Add(route);
				}
			}
		}

		sealed class StaticNode {
			/// <summary>
			/// Part of the path of the segment.
			/// </summary>
			/// <value>The path.</value>
			public string Path {get; private set;}

			/// <summary>
			/// List of Routes which have futher static segements
			/// </summary>
			/// <value>The static segments.</value>
			public StaticSegments StaticSegments { get; private set; }

			/// <summary>
			/// List of all Routes which do not have further static segements
			/// </summary>
			/// <value>The dynamics.</value>
			public Leaves Leaves {get; set;}

			public StaticNode (string path)
			{
				Path = path;
				StaticSegments = new StaticSegments();
				Leaves = new Leaves();
			}

			public override bool Equals(object obj){
				if (!(obj is StaticNode))
					return false;

				return (obj as StaticNode).Path.Equals(this.Path);
			}

			public override int GetHashCode(){
				return Path.GetHashCode();
			}

			public override string ToString(){
				return Path;
			}
		}

		class StaticSegments {
			Dictionary<string, StaticNode> _nodes = new Dictionary<string, StaticNode>();

			public void Add(Segment node, out StaticNode target){
				if (!_nodes.ContainsKey(node.Value)) {
					lock (this) {
						if (!_nodes.ContainsKey(node.Value)) {
							_nodes.Add(node.Value, new StaticNode(node.Value));
						}
					}
				}

				target = _nodes[node.Value];
			}

			public StaticNode Get(string segment){
				if (!_nodes.ContainsKey(segment))
					return null;

				return _nodes[segment];
			}
		}

		class Leaves : IEnumerable<Route> {
			List<Route> _routes = new List<Route>();

			public void Add(Route route){
				_routes.Add(route);
			}

			public IEnumerator<Route> GetEnumerator(){
				return _routes.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
				return _routes.GetEnumerator();
			}
		}
	}
}