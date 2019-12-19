﻿///The Core namespace includes all the related modules to logically handle a Maze game and its components.
namespace FMaze.Core

///The data structure representing a maze instance.
type MazeType = {
    ///map of cells representing the structure of the maze
    map : CellType list;
    ///number of rows the maze is made up of
    rows: int
    ///number of columns the maze is made up of
    cols: int
    }

///<summary>The <c>Maze</c> module contains functions to operate on <code>MazeType</code> instances.</summary>
module Maze =
    open Utils

    ///<summary>Defines operations on cell lists.</summary>
    module MazeMap =
        ///<summary>Generate a map and initialize cells to their default value.</summary>
        ///<param name="rows">Number of rows of the map</param>
        ///<param name="cols">Number of columns of the map</param>
        ///<returns>A new map of <code>rows</code> rows and <code>cols</code> columns whose elements are <code>CellType</code> instances initialized to default value.</returns>
        let generate_map (rows:int) (cols:int) =
            List.init (rows*cols) (fun i -> Cell.ERROR_CELL)

        ///<summary>Replace the cell at the given index in the cell list with the new given cell instance.</summary>
        ///<param name="position">Index of the cell to replace</param>
        ///<param name="cell">The new cell to replace in the map</param>
        ///<param name="map">The cell map to operate on</param>
        ///<returns>The map with the new replaced cell.</returns>
        let replace_cell (position:int) (cell:CellType) (map: CellType list): CellType list = 
            map.[..(position-1)]@[cell]@map.[(position+1)..]
               
        ///<summary>Given a cell find the index of that cell in the given map.</summary>
        ///<param name="cell">The new cell to look for in the map</param>
        ///<param name="map">The cell map to search on for the cell</param>
        ///<returns>The index of the cell in the map, or <c>-1</c> if the cell is not present in the map.</returns>
        let find_cell (cell: CellType) (map:CellType list) :int  = 
            let rec find (cell:'A) (position:int) (map:'A list) = 
                match map with
                | [] -> -1
                | x::xs -> (if x <> cell then find cell (position+1) xs else position)
            find cell 0 map

        ///<summary>Remove the wall of adjacent cells.</summary>
        ///<param name="current">The current cell</param>
        ///<param name="next">The adjacent cell</param>
        ///<returns>A new map with the modified cells</returns>
        let remove_common_wall (map: CellType list) (current:int) (next:int) (cols:int) =
            let c_x,c_y = from_monodim_to_bidim current cols
            let n_x,n_y = from_monodim_to_bidim next cols
            let on_same_row = c_x = n_x
            let on_same_column = c_y = n_y
            let current_cell = map.[current]
            let next_cell = map.[next]
            if on_same_row && (c_y = (n_y-1)) then
                replace_cell current {current_cell with walls= {current_cell.walls with right=Walls.OPEN}} (replace_cell next {next_cell with walls= {next_cell.walls with left=Walls.OPEN}} map)
            elif on_same_row && (c_y = (n_y+1)) then
                replace_cell current {current_cell with walls= {current_cell.walls with left=Walls.OPEN}} (replace_cell next {next_cell with walls= {next_cell.walls with right=Walls.OPEN}} map)
            elif on_same_column && (c_x = (n_x-1)) then
                replace_cell current {current_cell with walls= {current_cell.walls with bottom=Walls.OPEN}} (replace_cell next {next_cell with walls= {next_cell.walls with top=Walls.OPEN}} map)
            elif on_same_column && (c_x = (n_x+1)) then
                replace_cell current {current_cell with walls= {current_cell.walls with top=Walls.OPEN}} (replace_cell next {next_cell with walls= {next_cell.walls with bottom=Walls.OPEN}} map)
            else
                map

     
    ///<summary>Gets the cell at the specified index in the given Maze. Index is specified as 1-dimensional.</summary>
    ///<param name="position">The index of the desired element in the maze</param>
    ///<param name="map">The maze to select the element from</param>
    ///<returns>The cell at the specified index</returns>
    let get_cell (position:int) (maze:MazeType):CellType =
        maze.map.[position]

    ///<summary>Gets the cell at the specified index in the given Maze. Index is specified as 1-dimensional.</summary>
    ///<param name="position">The index of the desired element in the maze</param>
    ///<param name="map">The maze to select the element from</param>
    ///<returns>The cell at the specified index</returns>
    let get_bi_cell (x:int) (y:int) (maze:MazeType):CellType = 
        let i= (from_bidim_to_monodim maze.rows maze.cols x y)
        in
        if i < 0 || i >= maze.cols*maze.rows then Cell.ERROR_CELL
        else maze.map.[i]
        
    ///<summary>Routines used in the maze generation process.</summary>
    module Generator =
        open MazeMap

        ///<summary>The seed used for generating random numbers for implementing randomness in the maze generator algorhythm</summary>
        let SEED = System.Random()
      
        ///<summary>Given a cell map, check if the map has been entirely explored, i.e. all the cells have been visited by the generator algorhythm.</summary>
        ///<param name="map">The cell map which the check is performed on</param>
        ///<returns><c>true</c> if the map has been entirely explored, <c>false</c> if not.</returns>
        let rec is_explored (map:CellType list):bool =
            List.forall (fun (elem:CellType) -> elem.visited) map

        ///<summary>Given a maze and a cell index return a list of indexes of the cell neighbours.</summary>
        ///<param name="index">Index of the base cell</param>
        ///<param name="maze">The maze where the cell is located in</param>
        ///<returns>A list of monodimensional indexes each one representing an unvisited neighbour of the cell at the given index.</returns>
        let get_unvisited_neighbours (index: int) (maze:MazeType) : int list=
            let r,c = from_monodim_to_bidim index maze.cols
            //System.Diagnostics.Debug.WriteLine("from_monodim_to_bidim index maze.cols = {0} {1}",r,c)
            //neighbours cells
            let top    = from_bidim_to_monodim maze.rows maze.cols (r-1) c 
            let right  = from_bidim_to_monodim maze.rows maze.cols r (c+1)
            let bottom = from_bidim_to_monodim maze.rows maze.cols (r+1) c 
            let left   = from_bidim_to_monodim maze.rows maze.cols r (c-1) 
            //System.Diagnostics.Debug.WriteLine("finding unvisited neighbours of {0}",index)
            //System.Diagnostics.Debug.WriteLine( "{0} {1} {2} {3}",top,right,bottom,left)
            //the neighbours    
            List.filter (fun (el:int) -> el <> -1 && not maze.map.[el].visited ) [top;right;bottom;left]
        
        ///<summary>Given a maze and a cell index return the number of unvisited neightbours for the given cell.</summary>
        ///<param name="index">Index of the base cell</param>
        ///<param name="maze">The maze where the cell is located in</param>
        ///<returns>The number of unvisited neighbours of the cell at the given index.</returns>
        let unvisited_neighbours_number (index:int) (maze:MazeType):int =
            let neighbours = get_unvisited_neighbours index maze
            neighbours.Length
        
        ///<summary>Given a maze and a cell index check if the given cell has unvisited neighbours.</summary>
        ///<param name="index">Index of the base cell</param>
        ///<param name="maze">The maze where the cell is located in</param>
        ///<returns><c>true</c> if the given cell has unvisited neighbours, <c>false</c> if not.</returns>
        let has_unvisited_neighbours (index:int) (maze:MazeType):bool =
            let neighbours = get_unvisited_neighbours index maze
            neighbours.Length <> 0
        
        ///<summary>The recursive backtracker algorhythm used for generating the maze. See <a href="https://en.wikipedia.org/wiki/Maze_generation_algorithm#Recursive_backtracker">Maze generation on Wikipedia</a> for details about how the algorhythm.</summary>
        ///<param name="maze">An maze initialized with default cells</param>
        ///<param name="start">The index of the cell representing the entry point of the recursive backtracker algorhythm </param>
        ///<returns>The maze with randomly generated paths.</returns>
        let recursive_backtracker (maze:MazeType) (start: int):MazeType = 
            let rec aux (stack:int list) (maze:MazeType) (current:int):MazeType =
                let current_cell = Cell.set_visited (get_cell current maze) true //just visited the cell
                let map = replace_cell (current) (current_cell) maze.map        //updated cell map
                if not (is_explored map) then
                    let unvisited_neighbours = get_unvisited_neighbours current maze //get neighbours of current cell
                    let unvisited_neighbours_number = unvisited_neighbours.Length
                    if (unvisited_neighbours_number) > 0 then      //if there are unvisited neighbours
                        let next = unvisited_neighbours.[SEED.Next(0,unvisited_neighbours_number)] //select one of the unvisited neighbours randomly
                        let removed_map = remove_common_wall map current next maze.cols //open the map
                        let new_stack = next::stack //push the neighbour in the stack
                        aux new_stack {maze with map=removed_map} next
                    else
                        let new_stack = List.tail stack
                        aux new_stack {maze with map=map} (List.head stack)
                else
                    {maze with map=map}
            aux [] maze start
     
        ///<summary>Generate a maze from a default initialized <c>Maze</c> instance.</summary>
        ///<param name="maze">A default initialized maze.</param>
        ///<returns>A new maze made of the concatenation of the 2 mazes.</returns>
        let generate (maze:MazeType): MazeType =
            recursive_backtracker maze 0

     
    ///<summary>Creates a new Maze from the given parameters.</summary>
    ///<returns>The maze with the given parameters</returns>
    let create (rows:int) (cols:int) : MazeType =
        Generator.generate {map = (MazeMap.generate_map rows cols) ; rows = rows; cols = cols} 
