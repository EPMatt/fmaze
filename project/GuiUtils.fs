﻿(*
*  FMaze - A .NET functional console game
*  (https://github.com/EPMatt/fmaze)
*
*  (C) 2020
*  Lorenzo Donatelli  (https://github.com/whitedemond)
*  Matteo Agnoletto   (https://github.com/EPMatt)
*  Matteo Libralesso  (https://github.com/Redz-CAiiF)
*  
*  For licensing conditions related to this project, see LICENSE
*
*)

///The GUI namespace includes all the related modules to handle a Maze game through a graphical user interface
namespace FMaze.GUI


///<summary>Provide utility functions for handling the graphical user interface.</summary>
module Utils =
    open LabProg2019.Engine
    open LabProg2019.Gfx
    open LabProg2019
    ///Color used for banners background
    let BANNER_BACKGROUND = Color.DarkGray
    ///Color used for rendering text in banners
    let BANNER_FOREGROUND = Color.Yellow

    ///<summary>Render a simple banner with the given parameters on the center of the screen</summary>
    let render_banner (engine : engine) (text : string list) : sprite =
        //screen size
        let w,h = engine.screen_width, engine.screen_height
        //banner size
        let banner_width, banner_height = (List.max (List.map (fun (e:string) -> e.Length) text))+2,text.Length+2
        let fill_px = pixel.create(''', BANNER_BACKGROUND,BANNER_BACKGROUND)
        //create banner
        let banner = engine.create_and_register_sprite (image.rectangle (banner_width,banner_height, fill_px,fill_px),w/2-banner_width/2,h/2-banner_height/2, 1000)
        //print text in the banner
        List.iteri (fun i e ->  banner.draw_text(e,(banner_width - e.Length)/2,1+i, BANNER_FOREGROUND, BANNER_BACKGROUND)
                                String.iteri (fun j c -> if c = ' ' then banner.draw_text("@",(banner_width - e.Length)/2+j,1+i, BANNER_BACKGROUND, BANNER_BACKGROUND) else ()) e) (text)
        banner
