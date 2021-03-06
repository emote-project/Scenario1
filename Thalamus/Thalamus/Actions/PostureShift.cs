﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Thalamus.Actions
{
    public class PostureShift : Posture
    {
		public PostureShift(string id) : base(id) { }
		public PostureShift() : base("PostureShift" + Counter++) { }
        public PostureShift(string id, SyncPoint startTime) : this(id, startTime, SyncPoint.Null) { }
        public PostureShift(SyncPoint startTime) : this("PostureShift" + Counter++, startTime, SyncPoint.Null) { }
        public PostureShift(SyncPoint startTime, SyncPoint endTime) : this("PostureShift" + Counter++, startTime, endTime) { }
        public PostureShift(string id, SyncPoint startTime, SyncPoint endTime) : base(id, startTime, endTime) { }
    }
}
