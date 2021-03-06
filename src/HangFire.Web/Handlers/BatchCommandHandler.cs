// This file is part of HangFire.
// Copyright � 2013-2014 Sergey Odinokov.
// 
// HangFire is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as 
// published by the Free Software Foundation, either version 3 
// of the License, or any later version.
// 
// HangFire is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public 
// License along with HangFire. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Net;
using System.Web;

namespace HangFire.Web
{
    internal class BatchCommandHandler : GenericHandler
    {
        private readonly Action<string> _command;

        public BatchCommandHandler(Action<string> command)
        {
            _command = command;
        }

        public override void ProcessRequest()
        {
            var request = HttpContext.Current.Request;
            var jobIds = request.Form.GetValues("jobs[]");

            if (jobIds == null)
            {
                Response.StatusCode = 422;
                return;
            }

            foreach (var jobId in jobIds)
            {
                _command(jobId);
            }

            Response.StatusCode = (int)HttpStatusCode.NoContent;
        }
    }
}