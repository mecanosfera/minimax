﻿using System;
using System.Collections.Generic;
namespace Minimax
{
	public class Player
	{

		public int playerNumber;
		public int opponentNumber;
		public bool npc;
		public int wins = 0;
		public int randomness = 0;
        public Game1 game;

		public Player(Game1 game, int number, bool npc, int random=0)
		{
			playerNumber = number;
			if (playerNumber == 1)
			{
				opponentNumber = 2;
			}
			else 
			{ 
				opponentNumber = 1;
			}

			randomness = random;
			this.npc = npc;
            this.game = game;

		}

		public int[] bestMove(bool alphabeta=false)
		{

			Board board = game.board;
			int[] nextMove = new int[2] { -1, -1 };
			int bestVal = -10;

			//calcula a chance de uma escolha aleatória a partir do nível de randomness do jogador
			Random r = new Random ();
			int option = r.Next (1, 101);
			Console.WriteLine (option);
			if (randomness == 0 || option > randomness) {
				for (int y = 0; y < 3; y++) {
					for (int x = 0; x < 3; x++) {
						if (board.cell [x, y] == 0) {						
							Board myCopy = board.GetCopy ();
							myCopy.cell [x, y] = playerNumber;
							if (myCopy.Victory () == playerNumber) {
								nextMove [0] = x;
								nextMove [1] = y;
								return nextMove;
							}
							int val;
							if (alphabeta) {
								val = MinimaxAB (myCopy, board.getLeft (), false, 9999, -9999);
							} else {
								val = Minimax (myCopy, board.getLeft (), false);
							}
							if (val >= bestVal) {
								bestVal = val;
								nextMove [0] = x;
								nextMove [1] = y;
							}

						}
					}
				}
			} else {
				//escolhe aleatoriamente uma das células livres 
				int next = r.Next (0, board.getLeft ());
				for (int y = 0; y < 3; y++) {
					for (int x = 0; x < 3; x++) {
						if (board.cell[x, y] == 0) {
							if (next == 0) {
								nextMove[0] = x;
								nextMove[1] = y;
								return nextMove;
							}
							next--;
						}
					}
				}
			}
			return nextMove;
		}

		public int MinimaxAB(Board Board, int depth, bool isMax, int alpha, int beta){

			return 0;
		}


		public int Minimax(Board board, int depth, bool isMax)
		{
			if(board.IsGameOver() || depth == 0)
			{
				return board.GetScore(this);
			}


			int value;


			if(!isMax)
			{
				value = 9999999;

				List<Board> possibilities = board.GetPossibilities(opponentNumber);
				foreach(Board p in possibilities)
				{
					value = Math.Min(value, Minimax(p, depth - 1, true));
				}

			} else
			{
				value = -9999999;

				List<Board> possibilities = board.GetPossibilities(playerNumber);
				foreach (Board p in possibilities)
				{
					value = Math.Max(value, Minimax(p, depth - 1, false));
				}
			}

			return value;
		}
			
	}
}