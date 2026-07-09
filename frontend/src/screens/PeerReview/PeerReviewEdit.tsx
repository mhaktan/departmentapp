import React, { useState, useEffect } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

type PeerReviewRecord = {
  id: string | number;
  reviewerName?: string;
  score?: number;
  strengths?: string;
  improvements?: string;
  isAnonymous: boolean;
  performanceReviewId: string;
  employeeId: string;
};

interface PeerReviewEditProps {
  record: PeerReviewRecord | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const PeerReviewEdit: React.FC<PeerReviewEditProps> = ({ record, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<PeerReviewRecord>>(record ?? {});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  useEffect(() => {
    if (record) setForm({ ...record });
  }, [record]);

  const mutation = useMutation({
    mutationFn: (values: Partial<PeerReviewRecord>) =>
      dataProvider.update('PeerReview', record!.id, values),
    onSuccess: (_data, values) => { triggerFlows('update', 'PeerReview', values as Record<string, unknown>); onSuccess(); onClose(); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!record) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Edit PeerReview</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkInput mode="text" label="Reviewer Name" value={String(form.reviewerName ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('reviewerName', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Score" value={String(form.score ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('score', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Strengths" value={String(form.strengths ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('strengths', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Improvements" value={String(form.improvements ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('improvements', v))(e.detail)} />
                </div>
                <div>
                  <label style={{ display: 'flex', alignItems: 'center', gap: 8, fontSize: 14 }}>
                    <input type="checkbox" checked={!!form.isAnonymous} onChange={(e) => ((v) => setField('isAnonymous', v))(e.target.checked)} style={{ width: 16, height: 16 }} />
                    Is Anonymous *
                  </label>
                </div>
                <div>
                  <LookupSelect label="Performans Değerlendirmesi *" resource="PerformanceReview" value={String(form.performanceReviewId ?? '')} onChange={(v) => setField('performanceReviewId', v)} displayField="reviewPeriod" />
                </div>
                <div>
                  <LookupSelect label="Personel *" resource="Employee" value={String(form.employeeId ?? '')} onChange={(v) => setField('employeeId', v)} displayField="employeeNumber" />
                </div>
            </div>
          </div>
          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, padding: '16px 28px', borderTop: '1px solid #e8e8e8', flexShrink: 0, background: '#fff' }}>
            <TkButton label="Cancel" variant="secondary" onTkClick={onClose} />
            <TkButton label={mutation.isPending ? 'Saving…' : 'Save Changes'} variant="primary" mode="submit" disabled={mutation.isPending} />
          </div>
        </form>
      </div>
    </div>
  );
};
