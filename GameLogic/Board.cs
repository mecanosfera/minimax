﻿using System;
using System.Collections.Generic;

namespace Minimax{
	
	public class Board{
		
		public int[,] cell;
		public int size;
		public bool superTicTacToeDiagonalFromHell=true;

		public Board(int s=3){
			size = s;
			cell = new int[size, size];
			Clear();
		}
			
		public void Clear(){
			for (int y = 0; y < size; y++){
				for (int x = 0; x < size; x++){
					cell[x, y] = 0;
				}
			}
		}
			
        public int Victory(){
			if(superTicTacToeDiagonalFromHell) {
				if(CheckStateDiag(cell, 1, true)) {
					return 1;
				}
				if(CheckStateDiag(cell, 2, true)) {
					return 2;
				}
				if (getLeft() == 0)
				{
					return 3;
				}
				return 0;
			} else {
				return CheckState(cell);
			}
        }

		public void UpdateSize(int s){
			size = s;
			cell = new int[size, size];
		}


        public int getLeft(){
            int left = 0;
            for (int y = 0; y < size; y++){
                for (int x = 0; x < size; x++){
                    if (cell[x, y] == 0){
                        left++;
                    }
                }
            }
            return left;
        }

		public bool CheckStateDiag(int[,] cells, int pNumber, bool left){
			int delta = 3;
			int diagonais = ((2 * size) + ((size - 5) * 2))/2;
			bool inverte = false;
			bool exit = true;
			int s = size;
			if(size == 3) {
				s += 2;
			}/* else if(size == 4) {
				s += 1;
			}*/
			
			if(left) {
				for(int d = 0; d < diagonais; d++) {
					for(int i = delta; i < s; i++) {
						int x;
						int y;
						if(!inverte) {
							y = i;
							x = y - delta;

						} else {
							x = i;
							y = x - delta;
						}
						if(cells[x, y] != pNumber) {
							exit = false;
							break;
						}
					}

					if(exit) {
						return true;
					} else {
						exit = true;
					}

					if(delta == 0 && !inverte) {
						inverte = true;
						delta = 3;
					}
					delta--;
				}
			} else {
			
			}
			return false; 
			
		}

       
		public int CheckState(int[,] cells) {
			if(size == 3) {
				for(int y = 0; y < size; y++) {
					if(cells[0, y] == cells[1, y] && cells[1, y] == cells[2, y]) {
						return cells[0, y];
					}
				}
				for(int x = 0; x < size; x++) {
					if(cells[x, 0] == cells[x, 1] && cells[x, 1] == cells[x, 2]) {
						return cells[x, 0];
					}
				}
				if((cells[0, 0] == cells[1, 1] && cells[1, 1] == cells[2, 2]) || (cells[0, 2] == cells[1, 1] && cells[1, 1] == cells[2, 0])) {
					return cells[1, 1];
				}
			} else {				
				for(int y = 0; y < size; y++) {
					bool res = true;
					for(int x = 1; x < size; x++) {
						if(cells[x - 1, y] != cells[x, y]) {
							res = false;
							break;
						}
					}
					if(res){
						return cells[0, y];
					}
				}
				for(int x = 0; x < size; x++) {
					bool res = true;
					for(int y = 1; y < size; y++) {
						if(cells[x, y-1] != cells[x, y]) {
							res = false;
							break;
						}
					}
					if(res){
						return cells[x, 0];
					}
				}
				bool dlres = true;
				for(int dl = 1; dl < size; dl++) {
					if(cells[dl - 1, dl - 1] != cells[dl, dl]) {
						dlres = false;
						break;
					}
				}
				if(dlres) {
					return cells[0, 0];
				}
				bool drres = true;
				for(int dr = 1; dr < size; dr++) {
					if(cells[dr - 1, size - dr] != cells[dr, size - (dr + 1)]) {
						drres = false;
						break;
					}
				}
				if(drres) {
					return cells[0, size-1];
				}
			}

			if (getLeft() == 0)
			{
				return 3;
			}
			return 0;
		}

        public bool IsGameOver(){
            if (Victory()>0){
                return true;
            }
            return false;
        }


		public int GetScore(Player player){
            int winner = Victory();

			if (winner == player.playerNumber)
                return 1;
			else if (winner == player.opponentNumber)
                return -1;
            else
                return 0;
        }


        public Board GetCopy(){
            Board myCopy = new Board(size);

            for (int y = 0; y < size; y++){
                for (int x = 0; x < size; x++){
                    myCopy.cell[x, y] = cell[x, y];
                }
            }
            return myCopy;
        }
			

		public List<Board> GetPossibilities(int player){
            List<Board> result = new List<Board>();

            for (int y = 0; y < size; y++){
                for (int x = 0; x < size; x++){
                    if (cell[x, y] == 0){
                        Board myCopy = GetCopy();
						myCopy.cell[x, y] = player;
                        result.Add(myCopy);
                    }
                }
            }
            return result;
        }



	}
}
