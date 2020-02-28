<template>
<div>
      <div class="modal fade" id="edit" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                  <div class="modal-content">
                        <div class="modal-header">
                        <h4 class="modal-title" id="myModalLabel">Pridėti komentarą</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        
                        </div>
                        <form action="/create" method="post">
                              <div class="modal-body">
                                          <input type="hidden" name="ad_id" id="ad_id" value="">
                              <div class="form-group">
                                    <label for="comm">Komentaras</label>
                                    <textarea name="comment" id="comment" class="form-control"></textarea>
                              </div>
                              </div>
                              <div class="modal-footer">
                              <button type="button" class="btn btn-default" data-dismiss="modal">Uždaryti</button>
                              <button type="submit" class="btn btn-primary">Išsaugoti</button>
                              </div>
                        </form>
                  </div>
            </div>
      </div>
</div>
</template>