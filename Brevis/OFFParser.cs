using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class OFFParser
    {
        public readonly List<Triangle3D> Triangles;
        public OFFParser(string path)
        {
            /*
             * Currently, this is a very limited parser. It expects following lines: [OFF header, stats, vertices..., triangles...].
             * It doesn't handle any other polygons.
             * It doesn't handle comments or redundant new lines.
             * There is no exception handling at this moment.
             *
             * Anyway, it is useful and a good foundation for further work.
             */
            using (var sr = new StreamReader(path))
            {
                sr.ReadLine(); /* read header and ignore it */

                /* read number of vertices and triangles */
                var line = sr.ReadLine();
                var counts = line.Split(' ');
                var verticesCount = int.Parse(counts[0]);
                var trianglesCount = int.Parse(counts[1]);

                var vertices = new List<Vertex3D>(verticesCount);
                this.Triangles = new List<Triangle3D>(trianglesCount);

                /* read vertices */
                for (var i = 0; i < verticesCount; i++)
                {
                    line = sr.ReadLine();
                    var rawCoordinates = line.Split(' ');
                    vertices.Add(new Vertex3D(
                        double.Parse(rawCoordinates[0]),
                        double.Parse(rawCoordinates[1]),
                        double.Parse(rawCoordinates[2])
                    ));
                }

                var rng = new Random(1337); /* Constant seed for reproducibility. */
                /* read triangles */
                for (var i = 0; i < trianglesCount; i++)
                {
                    line = sr.ReadLine();
                    var rawTriangle = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    this.Triangles.Add(new Triangle3D(
                        vertices[int.Parse(rawTriangle[1])],
                        vertices[int.Parse(rawTriangle[2])],
                        vertices[int.Parse(rawTriangle[3])],
                        rng.Next(0x000000, 0x1000000)
                    ));
                }
            }

            /*
             * Approximate normals in vertices by probing adjacent triangles (and "home"-triangle).
             */
            foreach (var t1 in Triangles)
            {
                var adjacentNormalsA = new List<Vector3D>();
                var adjacentNormalsB = new List<Vector3D>();
                var adjacentNormalsC = new List<Vector3D>();
                foreach (var t2 in Triangles)
                {
                    if (t2.Contains(t1.a))
                        adjacentNormalsA.Add(t2.normal);
                    if (t2.Contains(t1.b))
                        adjacentNormalsB.Add(t2.normal);
                    if (t2.Contains(t1.c))
                        adjacentNormalsC.Add(t2.normal);
                }

                t1.a.SetNormal(Average(adjacentNormalsA));
                t1.b.SetNormal(Average(adjacentNormalsB));
                t1.c.SetNormal(Average(adjacentNormalsC));
            }
        }

        private Vector3D Average(List<Vector3D> l)
        {
            var result = new Vector3D(0, 0, 0);
            foreach (var v in l)
            {
                result = result.Add(v);
            }

            return result.Mul(1.0 / l.Count);
        }

        public void MakeTransparent(int n)
        {
            var rng = new Random(1337); /* Constant seed to be deterministic. */
            var shuffled = Triangles.OrderBy(a => rng.Next());
            int count = 0;
            foreach (var triangle in shuffled)
            {
                triangle.SetTransparent(count++ < n);
            }
        }
    }
}
