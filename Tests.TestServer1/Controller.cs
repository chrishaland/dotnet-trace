﻿namespace Tests.TestServer1;

[Route("")]
public class Controller : ControllerBase
{
    private readonly TraceMetadata _traces;

    public Controller(TraceMetadata traces)
    {
        _traces = traces;
    }

    public async Task<ActionResult<TraceMetadata>> Get()
    {
        await Task.CompletedTask;
        return new JsonResult(_traces);
    }
}
