import React, { useState, useEffect } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkDatepicker, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

type EmployeeRecord = {
  id: string | number;
  employeeNumber: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  birthDate?: string;
  gender?: string;
  nationalId?: string;
  address?: string;
  hireDate: string;
  terminationDate?: string;
  jobTitle?: string;
  employmentType: string;
  status: string;
  emergencyContactName?: string;
  emergencyContactPhone?: string;
  emergencyContactRelation?: string;
  bankAccountNumber?: string;
  bankName?: string;
  taxNumber?: string;
  socialSecurityNumber?: string;
  annualLeaveBalance?: number;
  notes?: string;
  departmentId: string;
  branchId: string;
  employeeId: string;
  onboardingId: string;
};

interface EmployeeEditProps {
  record: EmployeeRecord | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const EmployeeEdit: React.FC<EmployeeEditProps> = ({ record, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<EmployeeRecord>>(record ?? {});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  useEffect(() => {
    if (record) setForm({ ...record });
  }, [record]);

  const mutation = useMutation({
    mutationFn: (values: Partial<EmployeeRecord>) =>
      dataProvider.update('Employee', record!.id, values),
    onSuccess: (_data, values) => { triggerFlows('update', 'Employee', values as Record<string, unknown>); onSuccess(); onClose(); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!record) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Edit Employee</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkInput mode="text" label="Employee Number *" value={String(form.employeeNumber ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('employeeNumber', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="First Name *" value={String(form.firstName ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('firstName', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Last Name *" value={String(form.lastName ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('lastName', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Email *" value={String(form.email ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('email', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Phone" value={String(form.phone ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('phone', v))(e.detail)} />
                </div>
                <div>
                  <TkDatepicker label="Birth Date" value={String(form.birthDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('birthDate', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Gender" value={String(form.gender ?? '')} onChange={(v) => setField('gender', v ? Number(v) : null)} searchable={false} options={[{ label: 'Male', value: '0' }, { label: 'Female', value: '1' }, { label: 'Other', value: '2' }]} />
                </div>
                <div>
                  <TkInput mode="text" label="National Id" value={String(form.nationalId ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('nationalId', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Address" value={String(form.address ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('address', v))(e.detail)} />
                </div>
                <div>
                  <TkDatepicker label="Hire Date *" value={String(form.hireDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('hireDate', v))(e.detail)} />
                </div>
                <div>
                  <TkDatepicker label="Termination Date" value={String(form.terminationDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('terminationDate', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Job Title" value={String(form.jobTitle ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('jobTitle', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Employment Type *" value={String(form.employmentType ?? '')} onChange={(v) => setField('employmentType', v ? Number(v) : null)} searchable={false} options={[{ label: 'FullTime', value: '0' }, { label: 'PartTime', value: '1' }, { label: 'Contract', value: '2' }, { label: 'Intern', value: '3' }]} />
                </div>
                <div>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: 'Active', value: '0' }, { label: 'OnLeave', value: '1' }, { label: 'Terminated', value: '2' }, { label: 'Suspended', value: '3' }]} />
                </div>
                <div>
                  <TkInput mode="text" label="Emergency Contact Name" value={String(form.emergencyContactName ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('emergencyContactName', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Emergency Contact Phone" value={String(form.emergencyContactPhone ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('emergencyContactPhone', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Emergency Contact Relation" value={String(form.emergencyContactRelation ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('emergencyContactRelation', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Bank Account Number" value={String(form.bankAccountNumber ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('bankAccountNumber', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Bank Name" value={String(form.bankName ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('bankName', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Tax Number" value={String(form.taxNumber ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('taxNumber', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Social Security Number" value={String(form.socialSecurityNumber ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('socialSecurityNumber', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Annual Leave Balance" value={String(form.annualLeaveBalance ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('annualLeaveBalance', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Notes" value={String(form.notes ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('notes', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Departman *" resource="Department" value={String(form.departmentId ?? '')} onChange={(v) => setField('departmentId', v)} />
                </div>
                <div>
                  <LookupSelect label="Şube *" resource="Branch" value={String(form.branchId ?? '')} onChange={(v) => setField('branchId', v)} />
                </div>
                <div>
                  <LookupSelect label="Personel *" resource="Employee" value={String(form.employeeId ?? '')} onChange={(v) => setField('employeeId', v)} displayField="employeeNumber" />
                </div>
                <div>
                  <LookupSelect label="İşe Alım / Oryantasyon *" resource="Onboarding" value={String(form.onboardingId ?? '')} onChange={(v) => setField('onboardingId', v)} displayField="notes" />
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
