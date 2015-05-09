using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpackRunner
{
	public class BasicShape
	{
		public Vector3 Size;
		public Vector3 Position;
		public VertexPositionNormalTexture[] vertexes;
		private int shapeTriangles;
		private VertexBuffer shapeBuffer;
		public Texture2D shapeTexture;

		public BasicShape (Vector3 size, Vector3 position)
		{
			Size = size;
			Position = position;
			ConstructCube ();
		}

		private void ConstructCube ()
		{
			// Calculate the position of the vertices on the top face.
			Vector3 topLeftFront = Position + new Vector3 (-1.0f, 1.0f, -1.0f) * Size;
			Vector3 topLeftBack = Position + new Vector3 (-1.0f, 1.0f, 1.0f) * Size;
			Vector3 topRightFront = Position + new Vector3 (1.0f, 1.0f, -1.0f) * Size;
			Vector3 topRightBack = Position + new Vector3 (1.0f, 1.0f, 1.0f) * Size;

			// Calculate the position of the vertices on the bottom face.
			Vector3 btmLeftFront = Position + new Vector3 (-1.0f, -1.0f, -1.0f) * Size;
			Vector3 btmLeftBack = Position + new Vector3 (-1.0f, -1.0f, 1.0f) * Size;
			Vector3 btmRightFront = Position + new Vector3 (1.0f, -1.0f, -1.0f) * Size;
			Vector3 btmRightBack = Position + new Vector3 (1.0f, -1.0f, 1.0f) * Size;

			// Normal vectors for each face (needed for lighting / display)
			Vector3 normalFront = new Vector3 (0.0f, 0.0f, 1.0f) * Size;
			Vector3 normalBack = new Vector3 (0.0f, 0.0f, -1.0f) * Size;
			Vector3 normalTop = new Vector3 (0.0f, 1.0f, 0.0f) * Size;
			Vector3 normalBottom = new Vector3 (0.0f, -1.0f, 0.0f) * Size;
			Vector3 normalLeft = new Vector3 (-1.0f, 0.0f, 0.0f) * Size;
			Vector3 normalRight = new Vector3 (1.0f, 0.0f, 0.0f) * Size;

			// UV texture coordinates
			Vector2 textureTopLeft = new Vector2 (1.0f * Size.X, 0.0f * Size.Y);
			Vector2 textureTopRight = new Vector2 (0.0f * Size.X, 0.0f * Size.Y);
			Vector2 textureBottomLeft = new Vector2 (1.0f * Size.X, 1.0f * Size.Y);
			Vector2 textureBottomRight = new Vector2 (0.0f * Size.X, 1.0f * Size.Y);

			vertexes = new VertexPositionNormalTexture[36];
			Vector2 Texcoords = new Vector2 (0f, 0f);

			// Add the vertices for the FRONT face.
			vertexes [0] = new VertexPositionNormalTexture (topLeftFront, normalFront, textureTopLeft);
			vertexes [1] = new VertexPositionNormalTexture (btmLeftFront, normalFront, textureBottomLeft);
			vertexes [2] = new VertexPositionNormalTexture (topRightFront, normalFront, textureTopRight);
			vertexes [3] = new VertexPositionNormalTexture (btmLeftFront, normalFront, textureBottomLeft);
			vertexes [4] = new VertexPositionNormalTexture (btmRightFront, normalFront, textureBottomRight);
			vertexes [5] = new VertexPositionNormalTexture (topRightFront, normalFront, textureTopRight);

			// Add the vertices for the BACK face.
			vertexes [6] = new VertexPositionNormalTexture (topLeftBack, normalBack, textureTopRight);
			vertexes [7] = new VertexPositionNormalTexture (topRightBack, normalBack, textureTopLeft);
			vertexes [8] = new VertexPositionNormalTexture (btmLeftBack, normalBack, textureBottomRight);
			vertexes [9] = new VertexPositionNormalTexture (btmLeftBack, normalBack, textureBottomRight);
			vertexes [10] = new VertexPositionNormalTexture (topRightBack, normalBack, textureTopLeft);
			vertexes [11] = new VertexPositionNormalTexture (btmRightBack, normalBack, textureBottomLeft);

			//vertexese vertices for the TOP face.
			vertexes [12] = new VertexPositionNormalTexture (topLeftFront, normalTop, textureBottomLeft);
			vertexes [13] = new VertexPositionNormalTexture (topRightBack, normalTop, textureTopRight);
			vertexes [14] = new VertexPositionNormalTexture (topLeftBack, normalTop, textureTopLeft);
			vertexes [15] = new VertexPositionNormalTexture (topLeftFront, normalTop, textureBottomLeft);
			vertexes [16] = new VertexPositionNormalTexture (topRightFront, normalTop, textureBottomRight);
			vertexes [17] = new VertexPositionNormalTexture (topRightBack, normalTop, textureTopRight);

			//vertexese vertices for the BOTTOM face. 
			vertexes [18] = new VertexPositionNormalTexture (btmLeftFront, normalBottom, textureTopLeft);
			vertexes [19] = new VertexPositionNormalTexture (btmLeftBack, normalBottom, textureBottomLeft);
			vertexes [20] = new VertexPositionNormalTexture (btmRightBack, normalBottom, textureBottomRight);
			vertexes [21] = new VertexPositionNormalTexture (btmLeftFront, normalBottom, textureTopLeft);
			vertexes [22] = new VertexPositionNormalTexture (btmRightBack, normalBottom, textureBottomRight);
			vertexes [23] = new VertexPositionNormalTexture (btmRightFront, normalBottom, textureTopRight);

			//vertexese vertices for the LEFT face.
			vertexes [24] = new VertexPositionNormalTexture (topLeftFront, normalLeft, textureTopRight);
			vertexes [25] = new VertexPositionNormalTexture (btmLeftBack, normalLeft, textureBottomLeft);
			vertexes [26] = new VertexPositionNormalTexture (btmLeftFront, normalLeft, textureBottomRight);
			vertexes [27] = new VertexPositionNormalTexture (topLeftBack, normalLeft, textureTopLeft);
			vertexes [28] = new VertexPositionNormalTexture (btmLeftBack, normalLeft, textureBottomLeft);
			vertexes [29] = new VertexPositionNormalTexture (topLeftFront, normalLeft, textureTopRight);

			//vertexese vertices for the RIGHT face. 
			vertexes [30] = new VertexPositionNormalTexture (topRightFront, normalRight, textureTopLeft);
			vertexes [31] = new VertexPositionNormalTexture (btmRightFront, normalRight, textureBottomLeft);
			vertexes [32] = new VertexPositionNormalTexture (btmRightBack, normalRight, textureBottomRight);
			vertexes [33] = new VertexPositionNormalTexture (topRightBack, normalRight, textureTopRight);
			vertexes [34] = new VertexPositionNormalTexture (topRightFront, normalRight, textureTopLeft);
			vertexes [35] = new VertexPositionNormalTexture (btmRightBack, normalRight, textureBottomRight);

		}

		public void RenderShape (GraphicsDevice device)
		{
			ConstructCube ();
			// Create the shape buffer and dispose of it to prevent out of memory
			using (var buffer = new VertexBuffer (
				                    device,
				                    VertexPositionNormalTexture.VertexDeclaration,
				                    36,
				                    BufferUsage.WriteOnly)) {
				// Load the buffer
				buffer.SetData (vertexes);
			  
				// Send the vertex buffer to the device
				device.SetVertexBuffer (buffer);
			}
			
			// Draw the primitives from the vertex buffer to the device as triangles
			device.DrawPrimitives (PrimitiveType.TriangleList, 0, 12);  
		}
	}
}
