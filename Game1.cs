﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minimax
{
	public class Game1 : Game
	{
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public Board board = new Board();
		public Texture2D cellEmpty;
		public Texture2D cellX;
		public Texture2D cellO;
		public Texture2D gameDraw;
		public Player player1;
		public Player player2;
		public Player actualPlayer;

		public bool mousePressed = false;
		public int win = 0;
		public int AISpeed = 2000;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			player1 = new Player(this, 1, false);
			player2 = new Player(this, 2, true,0);
			actualPlayer = player1;
		}


		protected override void Initialize()
		{
			base.Initialize();
		}


		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			cellEmpty = Content.Load<Texture2D>("cell");
			cellX = Content.Load<Texture2D>("cellx");
			cellO = Content.Load<Texture2D>("cello");
			gameDraw = Content.Load<Texture2D>("Char33");
		}


		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (win == 0)
			{

				if (actualPlayer.npc)
				{
					
					int[] nextMove = actualPlayer.bestMove();
					if (nextMove [0] > -1) {
						board.cell [nextMove [0], nextMove [1]] = actualPlayer.playerNumber;
					}

					if (actualPlayer == player1)
					{
						actualPlayer = player2;
					}
					else
					{
						actualPlayer = player1;
					}
					win = board.Victory();
					Console.WriteLine(win);

				}
				else
				{
					if (Mouse.GetState().LeftButton == ButtonState.Pressed && !mousePressed)
					{
						mousePressed = true;
						Vector2 pTest = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

						for (int y = 0; y < 3; y++)
						{
							for (int x = 0; x < 3; x++)
							{
								Vector2 pMin = new Vector2(
									x * cellEmpty.Width + ((GraphicsDevice.Viewport.Width - cellEmpty.Width * 3) / 2),
									y * cellEmpty.Height + ((GraphicsDevice.Viewport.Height - (cellEmpty.Height * 3)) / 2)
								);
								Vector2 pMax = pMin + new Vector2(cellEmpty.Width, cellEmpty.Height);

								if (pTest.X >= pMin.X && pTest.X <= pMax.X && pTest.Y >= pMin.Y && pTest.Y <= pMax.Y)
								{
									if (actualPlayer.playerNumber == 1 && board.cell[x, y] == 0)
									{
										board.cell[x, y] = 1;
										actualPlayer = player2;
									}
									else if (actualPlayer.playerNumber == 2 && board.cell[x, y] == 0)
									{
										board.cell[x, y] = 2;
										actualPlayer = player1;
									}
								}
							}
						}
					}
					if (Mouse.GetState().LeftButton == ButtonState.Released && mousePressed)
					{
						mousePressed = false;
						win = board.Victory();
					}
				}
			}
			else
			{
				if (Mouse.GetState().LeftButton == ButtonState.Pressed && !mousePressed)
				{
					mousePressed = true;
					board.Clear();
					actualPlayer = player1;
					win = 0;
				}
				if (Mouse.GetState().LeftButton == ButtonState.Released && mousePressed)
				{
					mousePressed = false;
				}
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();


			if (win == 0)
			{

				for (int y = 0; y < 3; y++)
				{
					for (int x = 0; x < 3; x++)
					{

						Vector2 cellPos = new Vector2(
							x * cellEmpty.Width + ((GraphicsDevice.Viewport.Width - cellEmpty.Width * 3) / 2),
							y * cellEmpty.Height + ((GraphicsDevice.Viewport.Height - (cellEmpty.Height * 3)) / 2)
						);

						spriteBatch.Draw(
							cellEmpty,
							cellPos,
							null,
							null,
							new Vector2(0f, 0f), //pivot
							0.0f,                                             //rotation
							new Vector2(1f, 1f),                                //scale
							Color.White,                                        //color
							SpriteEffects.None,
							0f                                                  //depth
						);

						if (board.cell[x, y] == 1)
						{

							spriteBatch.Draw(
								cellO,
								cellPos,
								null,
								null,
								new Vector2(0f, 0f), //pivot
								0.0f
							);
						}
						else if (board.cell[x, y] == 2)
						{
							spriteBatch.Draw(
								cellX,
								cellPos,
								null,
								null,
								new Vector2(0f, 0f), //pivot
								0.0f
							);
						}
					}
				}
			}
			else
			{
				Texture2D winner = cellEmpty;
				if (win == 1)
				{
					winner = cellO;
				}
				else if (win == 2)
				{
					winner = cellX;
				}
				else if (win == 3)
				{
					winner = gameDraw;
				}
				spriteBatch.Draw(
					winner,
					new Vector2(
						((GraphicsDevice.Viewport.Width - cellEmpty.Width) / 2),
						((GraphicsDevice.Viewport.Height - (cellEmpty.Height)) / 2)
					),
					null,
					null,
					new Vector2(0f, 0f), //pivot
					0.0f,                                             //rotation
					new Vector2(1f, 1f),                                //scale
					Color.White,                                        //color
					SpriteEffects.None,
					0f                                                  //depth
				);
			}

			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}