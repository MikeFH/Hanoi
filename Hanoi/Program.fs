// Learn more about F# at http://fsharp.org

open System
open System.Threading

type Puzzle(state) =
    let poles : int list list = state
    
    member this.Print() =
        Console.Clear()
        for p in poles do
            printfn "%A" (List.rev p)
            Console.WriteLine()
        //leave the puzzle visible for some time
        Thread.Sleep 100

    member this.MovePiece sourcePoleIndex destinationPoleIndex =
        let piece = poles.[sourcePoleIndex].Head
        let p = new Puzzle([
            for i in 0..2 -> 
                if i = sourcePoleIndex then
                    List.skip 1 poles.[sourcePoleIndex]
                else if i = destinationPoleIndex then
                    piece :: poles.[destinationPoleIndex]
                else
                    poles.[i]
        ])        
        p.Print()
        p

    new() = Puzzle(8)
    new(height) = Puzzle([
        [1..height]
        []
        []
    ])

let Solve (height:int) = 
    let rec Move (puzzle:Puzzle) sourcePoleIndex destinationPoleIndex intemediatePoleIndex nbPiecesToMove =
        if nbPiecesToMove = 0 then
            puzzle
        else
            let p1 = Move puzzle sourcePoleIndex intemediatePoleIndex destinationPoleIndex (nbPiecesToMove - 1)
            let p2 = p1.MovePiece sourcePoleIndex destinationPoleIndex
            Move p2 intemediatePoleIndex destinationPoleIndex sourcePoleIndex (nbPiecesToMove - 1)
    let puzzle = new Puzzle(height)
    Move puzzle 0 2 1 height |> ignore

[<EntryPoint>]
let main argv =
    Solve 8
    Console.ReadLine() |> ignore
    0 // return an integer exit code