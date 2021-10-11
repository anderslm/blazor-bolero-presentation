namespace BoleroTddTemplate.Server

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
                return Path.Combine(env.ContentRootPath, "Presentation/" + pageNumber.ToString() + ".markdown") |> File.ReadAllText
            }
        }
