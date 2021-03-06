﻿(*
* LabProg2019 - Progetto di Programmazione a.a. 2019-20
* Config.fs: static configuration
* (C) 2019 Alvise Spano' @ Universita' Ca' Foscari di Venezia
*)

module LabProg2019.Config

open Prelude

///menu configuration
let MENU_WIDTH = 101
let MENU_HEIGHT = 30
let TITLE_X = 20
let TITLE_Y = 4
let COPYRIGHT_NOTICE = "(C) 2020 Lorenzo Donatelli, Matteo Agnoletto, Matteo Libralesso"

//maze configuration
let MAZE_DIMENSIONS = [(4,15);(10,20);(20,40);(25,60)] //different dimensions for different difficulties
let PLAYER_VISIBILITY_RANGE = 3     //player visibility in dark labyrinth mode. defines an nxn square around the player

//keys
let QUIT_KEY = 'q'  //used to quit a window and go back to the previous one in any point in the application

let filled_pixel_char = '*'
let empty_pixel_char = ' '

let default_flip_queue = 2  // double buffering
let default_fps_cap = 30

let log_pipe_name = "LogPipe"
let log_pipe_translate_eol = '\255'

let game_console_title = "Game Window"
let log_console_title = "Log Window"

let log_msg_color = Color.Gray
let log_warn_color = Color.Yellow
let log_error_color = Color.Red
let log_debug_color = Color.Cyan
