﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Timers;

namespace Minimax
{
	public class PlayGameState : GameState
	{

		//public List<CellButton> cells;
		public CellButton[,] cells;
		public int[] score = new int[3] {0,0,0}; //0=empate, 1=p1, 2=p2
		public int win=0;
		public TextElement scoreP1;
		public TextElement scoreP2;
		public TextElement scoreEmpate;
		public ButtonElement quitMenu;
		public bool timerEnd = false;
		public bool timerSet = false;
		public int timerStart = 0;
		public Timer _timer;
		public int actualBg = 0;
		public DivElement[] charLeft;
		public DivElement[] charRight;
		public TextElement[] charNameLeft;
		public TextElement[] charNameRight;


		public PlayGameState(Game1 g, string n): base(g,n)
		{
			cells = new CellButton[game.board.size,game.board.size];
			for (int y = 0; y < game.board.size; y++) {
				for (int x = 0; x < game.board.size; x++) {
					Vector2 cellPos = new Vector2(
						x * 72 + ((view.size.X - (72 * game.board.size)) / 2),
						y * 72 + ((view.size.Y-220 - (72 * game.board.size)))
					);

					DivElement cellBorder = new DivElement(
	                        game,
	                        new Vector2(72, 72),
	                        cellPos,
	                        game.cellEmpty
                        );
					cellBorder.position="absolute";
					cellBorder.backgroundType="cover";

					CellButton cell = new CellButton(
						game,
						new Vector2(72,72),
						cellPos,
						new int[2] {x,y}
					);

					cell.AddEventListener("click",delegate(Event e){
						CellButton c = (CellButton) e.target;
						if(win==0 && !game.actualPlayer.npc){
							if(game.board.cell[c.cell[0],c.cell[1]]==0){
								if((game.actualPlayer.playerNumber==1 && game.charactersP1=="empire") || (game.actualPlayer.playerNumber==2 && game.charactersP2=="empire")){
									game.kefkaPlay.Play();
								} else {
									game.itemSelectPlay.Play();
								}
								c.updateCell(game.actualPlayer.playerNumber);
								win = game.board.Victory();	
								Console.WriteLine("win = "+win);
							}
						}
					});
					cell.position = "absolute";

					view.Append(cellBorder);
					view.Append(cell);
					cells[x, y] = cell;
				}
			}

			scoreP1 = new TextElement(game,game.charactersP1.ToUpper()+": "+score[1]);
			scoreP2 = new TextElement(game,game.charactersP2.ToUpper()+": "+score[2]);
			scoreEmpate = new TextElement(game,"DRAW: "+score[0]);
			scoreP1.Margin(20);
			scoreP1.vAlign="top";
			scoreP2.Margin(20);
			scoreP1.align="left";
			scoreP2.align="right";
			scoreP2.vAlign="top";
			scoreP2.textAlign="right";
			scoreEmpate.Margin(20);
			scoreEmpate.align="center";
			scoreEmpate.vAlign="top";
			scoreEmpate.textAlign="center";

			quitMenu = new ButtonElement(game,"Quit to Menu");
			//quitMenu.font = game.ff6_32;
			quitMenu.Align("right", "bottom");
			quitMenu.Margin(30,10,50,8);
			quitMenu.Padding(40,0,0,0);
			quitMenu.AddEventListener("click",delegate(Event e) {
				game.GameMode.Change("title");
			});
			quitMenu.hover(game.hand);
			quitMenu.setSound();


			int topPos = 190;
			int leftPos = 580;
			int textPos = 460;
			charLeft = new DivElement[game.characters["returners"].Length];
			charNameLeft = new TextElement[game.characters["returners"].Length];

			for(int i=0;i<charLeft.Length;i++) {
				charLeft[i] = new DivElement(
					game,
					new Vector2(72,72),
					new Vector2(leftPos,topPos),
					game.spritesLeft[game.charactersP2]["profile"][i]
				);
				charLeft[i].position="absolute";
				charNameLeft[i] = new TextElement(game,game.characters[game.charactersP2][i].ToUpper(),game.ff6_32);
				charNameLeft[i].position="absolute";
				charNameLeft[i].Padding(20, 5, 5, 5);
				charNameLeft[i].pos = new Vector2(390,textPos);
				view.Append(charNameLeft[i]);
				view.Append(charLeft[i]);
				leftPos += 20;
				topPos += 55;
				textPos += 40;
			}

			topPos = 190;
			int rightPos = 60;
			textPos = 460;

			charRight = new DivElement[game.characters["returners"].Length];
			charNameRight = new TextElement[game.characters["returners"].Length];

			for(int i=0;i<charRight.Length;i++) {
				charRight[i] = new DivElement(
					game,
					new Vector2(72,72),
					new Vector2(rightPos,topPos),
					game.spritesRight[game.charactersP1]["profile"][i]
				);
				charRight[i].position="absolute";
				charNameRight[i] = new TextElement(game,game.characters[game.charactersP1][i].ToUpper(),game.ff6_32);
				charNameRight[i].position="absolute";
				charNameRight[i].Padding(20, 5, 5, 5);
				charNameRight[i].pos = new Vector2(30,textPos);
				view.Append(charNameRight[i]);
				view.Append(charRight[i]);
				rightPos -= 20;
				topPos += 55;
				textPos += 40;
			}
				
				
			view.Append(scoreP1);
			view.Append(scoreP2);
			view.Append(scoreEmpate);
			view.Append(quitMenu);


		}
			

		public override void Enter(string lastState=null){
			game.menuPlay.Stop();
			game.victoryPlay.Stop();
			game.battleStartPlay.Play();
			if(!game.board.superTicTacToeDiagonalFromHell) {
				game.battlePlay.Play();	
			} else {
				game.bossPlay.Play();
			}



			view.backgroundImage = game.battle_bg2[actualBg];
			game.board.Clear();
			foreach(CellButton cell in cells) {
				cell.Clear();
			}
			win = 0;
			game.actualPlayer = game.player1;
			if(lastState == "end") {
				score = ((EndGameState)game.GameMode.get("end")).score;
			} else {
				score = new int[3]{ 0, 0, 0 };
			}

			for(int i = 0; i < 4; i++) {
				charRight[i].backgroundImage = game.spritesRight[game.charactersP1]["profile"][i];
				charLeft[i].backgroundImage = game.spritesLeft[game.charactersP2]["profile"][i];
			}
			scoreP1.text = game.charactersP1.ToUpper() + ": " + score[1];
			scoreP1.text = game.charactersP2.ToUpper() + ": " + score[2];

		}

		public override void Update(){
			base.HandleInput();
			if(win == 0) {
				if(game.actualPlayer.npc){
					if(!timerEnd) {
						if(!timerSet) {
							_timer = new Timer(game.IADelay);
							timerStart++;
							_timer.Elapsed += new ElapsedEventHandler(delegate(object sender, ElapsedEventArgs e) {
								timerEnd = true;
								_timer.Enabled = false;
							});
							_timer.Enabled = true;
							timerSet = true;
						}
					} else {
						if((game.actualPlayer.playerNumber==1 && game.charactersP1=="empire") || (game.actualPlayer.playerNumber==2 && game.charactersP2=="empire")){
							game.kefkaPlay.Play();
						} else {
							game.itemSelectPlay.Play();
						}
						int[] nextMove;
						nextMove = game.actualPlayer.bestMove(game.alphabeta);
						if(nextMove[0] > -1) {
							cells[nextMove[0], nextMove[1]].updateCell(game.actualPlayer.playerNumber);
						} 
						win = game.board.Victory();
						timerEnd = false;
						timerSet = false;
					}
				}
			} else {
				if(win == 3) {
					score[0]++;
				} else if(win == 1) {
					score[1]++;
				} else {
					score[2]++;
				}
				game.GameMode.Change("end");
			}
			scoreEmpate.text = "DRAW: "+score[0];
			scoreP1.text = game.charactersP1.ToUpper()+": " + score[1];
			scoreP2.text = game.charactersP2.ToUpper()+": "+score[2];
		}


		public override void Exit(string newState=null){
			if(newState == "start") {
				score = new int[3] { 0, 0, 0 };
				game.board.Clear();
				win = 0;
				//game.player1.npc = false;
				//game.player2.npc = false;
				//game.player1.difficulty = 100;
				//game.player2.difficulty = 100;
				game.actualPlayer = game.player1;
			}


		}
			
	}
}

