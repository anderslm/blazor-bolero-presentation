namespace BoleroTddTemplate.Server

open System
open System.IO
open Microsoft.AspNetCore.Hosting
open Bolero.Remoting
open Bolero.Remoting.Server
open BoleroTddTemplate

type PresentationService(ctx: IRemoteContext, env: IWebHostEnvironment) =
    inherit RemoteHandler<Client.Presentation.Service>()

    override this.Handler =
        {
            getPresentation = fun pageNumber -> async {
                try
                    return Path.Combine(env.ContentRootPath, "Presentation/" + pageNumber.ToString() + ".markdown") |> File.ReadAllText
                with
                | _ ->
                    return String.Empty
            }
        }
